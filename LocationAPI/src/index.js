const app = require('./app')
const http = require('http').createServer(app);
const io = require('socket.io')(http);
const Coordinate = require('./models/coordinate')
var amqp = require('amqplib/callback_api');

var recieved = false;

function connect() {
   var rabbit = setInterval(() => {

      try {
         console.log('Connecting to rabbitmq.........')
         amqp.connect('amqp://rabbitmq', function(error0, connection) {
         if (error0) {
            throw error0;
         }
         connection.createChannel(function(error1, channel) {
            if (error1) {
               throw error1;
            }
            channel.assertQueue('', {
               exclusive: true
            }, function(error2, q) {
               if (error2) {
                  throw error2;
               }
               var correlationId = generateUuid();
               channel.consume(q.queue, function(msg) {
                  if (msg.properties.correlationId === correlationId) {
                     console.log('Recieved %s', msg.content.toString());
                     saveCoordinates(msg);
                     recieved = true;
                     clearInterval(rabbit)
                  }
               }, {
                  noAck: true
               });
               channel.sendToQueue('rpc_queue',
               Buffer.from('GetCoordinates'), {
                  correlationId: correlationId,
                  replyTo: q.queue
               }
               );
            });
         });
      });
      } catch (e) {
         console.log(e)
      }
   }, 10000)
}

function generateUuid() {
   return Math.random().toString() +
   Math.random().toString() +
   Math.random().toString();
}


const saveCoordinates = async (req) => {
   console.log('**********savking coordinates***********')
   const coords = JSON.parse(req.content)
   await Coordinate.deleteMany({});
   if (coords["Success"] == true) {
      coords["Data"].forEach(coord => {
         const toSave = new Coordinate({
            lineId: coord["LineId"], 
            x: coord["XCoordinate"],
            y: coord["YCoordinate"]
         })
         toSave.save()
      });
      console.log('Coordinates SAVED.')
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

readCoordinates = async () => {
   try {
      console.log('reading coooooooooooords')
      const response = await Coordinate.find()
      response.forEach(row => lineIds.push(row.lineId))
      lineIds = lineIds.filter((a, b) => lineIds.indexOf(a) === b)
      
      lineIds.forEach(lineId => {
         let coords = response.filter(coord => coord.lineId == lineId)
         console.log('za svaki line id: ' + coords)
         calcCoords = []
         simulateLocation(coords)
         lineCoordinatesMap.set(lineId, calcCoords)
         numOfCoords.set(lineId, calcCoords.length)
         lineCoordIterator.set(lineId, 0)
      })
      return response
   } catch (e) { 
      console.log('******Error during reading coordinates from db: \n' + e)
      return false
   }
}

locationSender = () => {
   console.log('sender')
   setInterval(() => {
      lineIds.forEach(id => {
         let i = lineCoordIterator.get(id)
         console.log('id ' + id + 'i ' + i)
         io.to(id.toString()).emit('location', lineCoordinatesMap.get(id)[i])
         i++
         lineCoordIterator.set(id, i)
         if (i == (numOfCoords.get(id) - 1)) {
            lineCoordIterator.set(id, 0)
            lineCoordinatesMap.set(id, lineCoordinatesMap.get(id).reverse())
         }
      })
   }, 400)
}

io.on("connection", socket => {   
   console.log('New user connected')
   socket.on('location', async (lineId) => {
      console.log('location')
      socket.leaveAll;
      socket.join(lineId);
   })

   socket.on('leave', room => {
      socket.leave(room)
   })

   socket.on('disconnect', () => {
      console.log('User disconnected');
      userConnected = false
    });
 });

http.listen(3000, async () => {
   console.log('Listening for sockets 3000');
   connect()
   var read = setInterval(async () => {
      if (recieved) {
         await readCoordinates().then( _ => locationSender())
         clearInterval(read)
      }
   }, 10000)
   //locationSender()
});

var numOfDeltas = 10 //150
var currentX
var currentY
var endX
var endY
var j

// Calculates coordinates throughout the whole route and saves into calcCoords
const simulateLocation = (coords) => {
   console.log('simulate loc')
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
const fs = require('fs');
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
         toSave.save()
      });
      console.log('Coordinates SAVED.')
   }
   else {
      console.log("Getting coordinates from RouteAPI FAILED")
   }
}



// amqp.connect('amqp://rabbitmq', function(error0, connection) {
//          if (error0) {
//             throw error0;
//          }
//          connection.createChannel(function(error1, channel) {
//             if (error1) {
//                throw error1;
//             }
//             channel.assertQueue('', {
//                exclusive: true
//             }, function(error2, q) {
//                if (error2) {
//                   throw error2;
//                }
//                var correlationId = generateUuid();
//                channel.consume(q.queue, function(msg) {
//                   if (msg.properties.correlationId === correlationId) {
//                      console.log('Recieved %s', msg.content.toString());
//                      saveCoordinates(msg);
//                      recieved = true;
//                   }
//                }, {
//                   noAck: true
//                });
//                channel.sendToQueue('rpc_queue',
//                Buffer.from('GetCoordinates'), {
//                   correlationId: correlationId,
//                   replyTo: q.queue
//                }
//                );
//             });
//          });
//       });