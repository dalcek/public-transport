const mongoose = require('mongoose')

mongoose.connect('mongodb://locationdb', {
    useNewUrlParser: true,
    useCreateIndex: true,
    useFindAndModify: false, 
    useUnifiedTopology: true
})
