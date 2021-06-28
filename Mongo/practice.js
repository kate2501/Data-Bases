document = {
    "Name":"Ivan",
    "Surname":"Ivanov",
    "Age":20,
    "Group":{
        "Name":"KM",
        "Number":5,
        "Base":"Room 341"
    }   
}

db.studs.insertOne(document)
document1 = {
    "Name":"Petr",
    "Surname":"Petrov",
    "Age":20
    }   


db.studs.insertOne(document1)
    
db.studs.insertOne({
    "Name":"ּשהשגםפ",
    "Surname":"Petrov",
    "Age":21,
    "Languages":["English","German"]
    }   )
    
 db.studs.update(
    {"Name": "Petr"},
    { $set: 
        { "Marks": 
            {
                "DU": 5, 
                "Matan": 5, 
                "Algebra": 5, 
                "Geoma": 5
            }
        } 
    }, 
    {upsert: false})
 
 db.studs.update(
    {"Name": "Ivan"},
    { $set: 
        { "Marks": 
            {
                "DU": 3, 
                "Matan": 5, 
                "Algebra": 5
            }
        } 
    }, 
    {upsert: false}) 
  
  
// queries
db.studs.find()
    
db.studs.find({
    $and: [
        {"Age": {$eq: 20}},
        {"Name": {$regex: "^I"}}
    ]})    
    
func = function() {
    if (this.Name == "Ivan") {
        return true
        }
    else {
        return false
        }
    }
db.studs.find(func)
    
db.studs.aggregate([
    {
        $group: 
            {
                _id: "Group.Name",
                avgAge: {$avg: "$Age"}
            }
    }
])
    
db.studs.aggregate([
    {
        $group: 
            {
                _id: "Group.Name",
                count: {$sum: 1}
            }
    }
])

fn=function(){return this.Age==20}
db.studs.find(fn)

db.students.find({ "Languages":true})

db.students.aggregate(
[
{
    $group:
    {
        _id:"$Group.Name",
        avgDU: {$avg:"$marks.DU"},
        minAlgebra: {$min:"$marks.Algebra"},
        totalAge: {$sum:"$Age"}
    }
}
])

//indexes
db.studs.createIndex( { "Name": 1 },
{
      unique: true,
      sparse: true,
      expireAfterSeconds: 3600
  })
  
db.studs.getIndexes()
  
