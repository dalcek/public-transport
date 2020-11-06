const app = require('./app')
const http = require('http').createServer(app);
const io = require('socket.io')(http);
const Coordinate = require('./models/coordinate')
var amqp = require('amqplib/callback_api');
const port = 6005


app.listen(port, () => {
   console.log('Server is up on port ' + port)
   //fakeCoordinates()
})

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

function generateUuid() {
   return Math.random().toString() +
      Math.random().toString() +
      Math.random().toString();
}

const saveCoordinates = async (req) => {
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

var numOfDeltas = 50 //150
var currentX
var currentY
var nextX
var nextY
var j
var routeCoords = []
var userConnected = false
var reverse = false

function wait(miliseconds) 
{
  var e = new Date().getTime() + (miliseconds);
  while (new Date().getTime() <= e) {}
}

io.on("connection", socket => {   
   console.log('New user connected')
   userConnected = true
   socket.on('location', async (lineId) => {
   console.log('location ' + lineId)
   reverse = false
   let coords = await readCoordinates(lineId)
      
   let i = 0;
   simulateLocation(coords)
   var foo = setInterval (function () {
      console.log('sending ' + i)
      if (userConnected == false) {
         clearInterval(foo)
      }
      if (!reverse) {
         i++
         if (i == (routeCoords.length - 1)) {
            reverse = true
         }
      }
      else {
         i--
         if (i == 0) {
            reverse = false
         }
      }
      socket.emit('location', routeCoords[i])
   }, 50) //300
   })

   socket.on('my message', (msg) => {
      console.log('message: ' + msg)
      socket.emit('my message', 'porukica')
   })

   socket.on('disconnect', () => {
      console.log('User disconnected');
      userConnected = false
    });
 });

 http.listen(3000, () => {
   console.log('Listening for sockets 3000');
});

readCoordinates = async (req) => {
   try {
      const response = await Coordinate.find({lineId: req})
      return response
   } catch (e) { 
      console.log('******Error during reading coordinates from db: \n' + e)
      return false
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

// Calculates coordinates throughout the whole route and saves into routeCoords
const simulateLocation = (coords) => {
   // Calls transition for every section of the route
   for (let i = 0; i < coords.length - 1; i++) {
      currentX = coords[i].x
      currentY = coords[i].y
      nextX = coords[i + 1].x
      nextY = coords[i + 1].y
      transition()
   }
}

// Caluculates the direction which marker is moved for every section
const transition = () => {
   j = 0
   deltaX = (nextX - currentX) / numOfDeltas
   deltaY = (nextY - currentY) / numOfDeltas
   moveMarker()
}

const moveMarker = () => {
   currentX += deltaX
   currentY += deltaY
   routeCoords.push(currentX.toString() + '|' + currentY.toString())
   if (j != numOfDeltas) {
      j++
      moveMarker()
   }
}
