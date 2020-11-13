const mongoose = require('mongoose')

mongoose.connect('mongodb://locationdb-deployment.locationdb-cluster-ip-service.default.svc.cluster.local', {
    useNewUrlParser: true,
    useCreateIndex: true,
    useFindAndModify: false, 
    useUnifiedTopology: true
})
