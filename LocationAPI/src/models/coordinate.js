const mongoose = require('mongoose')

const coordinateSchema = new mongoose.Schema({
   lineId: {
      type: Number,
      required: true
   },
   x: {
      type: Number,
      required: true
   },
   y: {
      type: Number,
      required: true
   }
})

const Coordinate = mongoose.model('Coordinate', coordinateSchema)
module.exports = Coordinate