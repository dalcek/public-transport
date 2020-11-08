const express = require('express')
const Test = require('../models/test')
const router = new express.Router()
router.get('/test', async (req, res) => {
   res.send('caos')
})
// '/tasts', auth, async (req, res
router.post('/tests', async (req, res) => {
   console.log(req.body)
   const test = new Test({
      ...req.body
   })
   try {
       await test.save()
       res.status(201).send(test)
   } catch (e) {
       res.status(400).send(e)
   }
})
module.exports = router