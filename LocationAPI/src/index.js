const app = require('./app')
const http = require('http').createServer(app);
const io = require('socket.io')(http);
const Coordinate = require('./models/coordinate')
var amqp = require('amqplib/callback_api');

app.listen(6005,() => {
   console.log('Server is up on port ' + 6005)
})

var recieved = false;
var connected = false;

function connect() {
   console.log('Connecting to rabbitmq...')
   // For running in docker use amqp://rabbitmq
   // For running in k8s use amqp://rabbitmq-cluster-ip-service
   amqp.connect('amqp://rabbitmq', function(error0, connection) {
      console.log('u connect')
      if (error0) {
         console.log('Connect error [RabbitMQ]')
         throw error0;
      }
      connection.createChannel(function(error1, channel) {
         console.log('u channel')
         if (error1) {
         console.log('Create channel error [RabbitMQ]')
            throw error1;
         }
         channel.assertQueue('', {
            exclusive: true
         }, function(error2, q) {
            if (error2) {
               throw error2;
            }
            connected = true;
            var correlationId = generateUuid();
            channel.consume(q.queue, function(msg) {
               if (msg.properties.correlationId === correlationId) {
                  console.log('Recieved %s', msg.content.toString());
                  saveCoordinates(msg);
                  readCoordinates()
                  locationSender()
                  recieved = true;
               }
            }, {
               noAck: true
            });
            // If there is no response for 3 mins, send coords request again
            var send = setInterval( () => {
               if (recieved == true) {
                  clearInterval(send)
               }
               channel.sendToQueue('rpc_queue', Buffer.from('GetCoordinates'), {
                  correlationId: correlationId,
                  replyTo: q.queue
               });
            }, 120000)
         });
      });
   });
}

function generateUuid() {
   return Math.random().toString() +
   Math.random().toString() +
   Math.random().toString();
}

var recv = [];

const saveCoordinates = async (req) => {
   console.log('***Saving coordinates')
   const coords = JSON.parse(req.content)
   if (coords["Success"] == true) {
      coords["Data"].forEach(coord => {
         const toSave = new Coordinate({
            lineId: coord["LineId"], 
            x: coord["XCoordinate"],
            y: coord["YCoordinate"]
         })
         recv.push(toSave)
      });
      console.log('***Coordinates SAVED.')
   }
   else {
      console.log("Getting coordinates from RouteAPI FAILED")
   }
}

// Number of coordinates per line
var numOfCoords = new Map()
var lineIds = []
var lineCoordinatesMap = new Map()
var lineCoordIterator = new Map()

var calcCoords;

readCoordinates = () => {
   try {
      const response = recv
      response.forEach(row => lineIds.push(row.lineId))
      lineIds = lineIds.filter((a, b) => lineIds.indexOf(a) === b)
      
      lineIds.forEach(lineId => {
         let coords = response.filter(coord => coord.lineId == lineId)
         calcCoords = []
         simulateLocation(coords)
         lineCoordinatesMap.set(lineId, calcCoords)
         numOfCoords.set(lineId, calcCoords.length)
         lineCoordIterator.set(lineId, 0)
      })
      return response
   } catch (e) { 
      console.log('***Error during reading coordinates from db: \n' + e)
      return false
   }
}

locationSender = () => {
   setInterval(() => {
      lineIds.forEach(id => {
         let i = lineCoordIterator.get(id)
         console.log('Id ' + id + ' i ' + i)
         io.to(id.toString()).emit('location', lineCoordinatesMap.get(id)[i])
         i++
         lineCoordIterator.set(id, i)
         if (i == (numOfCoords.get(id) - 1)) {
            lineCoordIterator.set(id, 0)
            lineCoordinatesMap.set(id, lineCoordinatesMap.get(id).reverse())
         }
      })
   }, 300)
}

io.on("connection", socket => {   
   console.log('New user connected')

   socket.on('location', (lineId) => {
      console.log('Joined room: ' + lineId)
      socket.leaveAll
      socket.join(lineId)
   })

   socket.on('leave', room => {
      console.log('On leave')
      socket.leave(room)
   })

   socket.on('disconnect', () => {
      console.log('User disconnected')
      userConnected = false
    });
 });

http.listen(3000, () => {
   console.log('Listening for sockets 3000')

   var rabbitConn = setInterval( () => {
      if (connected == false) {
         try {
            connect()
         } catch (e) {
            console.log('Connection with rabbit failed')
         }
      }
      else {
         clearInterval(rabbitConn)
      }
   }, 60000)

   // var read = setInterval( () => {
   //    console.log('Rabbit recv for interval: ' + recieved)
   //    if (recieved) {
   //       readCoordinates()
   //       clearInterval(read)
   //       locationSender()
   //    }
   // }, 10000)
});

var numOfDeltas = 150
var currentX
var currentY
var endX
var endY
var j

// Calculates coordinates throughout the whole route and saves into calcCoords
const simulateLocation = (coords) => {
   console.log('Simulate location')
   // Calls transition for every section of the route
   for (let i = 0; i < coords.length - 1; i++) {
      currentX = coords[i].x
      currentY = coords[i].y
      endX = coords[i + 1].x
      endY = coords[i + 1].y
      transition()
   }
}

// Caluculates the direction which marker is moved for every section
const transition = () => {
   j = 0
   deltaX = (endX - currentX) / numOfDeltas
   deltaY = (endY - currentY) / numOfDeltas
   moveMarker()
}

const moveMarker = () => {
   currentX += deltaX
   currentY += deltaY
   calcCoords.push(currentX.toString() + '|' + currentY.toString())
   if (j != numOfDeltas) {
      j++
      moveMarker()
   }
}

// For testing with coordinates from info.json, no database
const fs = require('fs')
const fakeCoordinates = () => {

   const dataBuffer = fs.readFileSync('info.json')
   const dataJSON = dataBuffer.toString()
   const coords = JSON.parse(dataJSON)
   console.log(coords)

   if (coords["Success"] == true) {
      coords["Data"].forEach(coord => {
         const toSave = new Coordinate({
            lineId: coord["LineId"], 
            x: coord["XCoordinate"],
            y: coord["YCoordinate"]
         })
         recv.push(toSave)
      });
      console.log('Coordinates SAVED.')
   }
   else {
      console.log("Getting coordinates from RouteAPI FAILED")
   }
}
