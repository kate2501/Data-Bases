use students

db.students.deleteMany({"Age":{$gt:10}})


document = {
    "Name" : "Edgar",
    "Surname" : "Vasilevski",
    "Age" : 18.0,
    "Group" : {
        "Name" : "KM",
        "Number" : 5.0,
        "Base" : "Room 341"
    },
    "Quotes" : [ 
        "sosi", 
        "pivka dlya ryvka"
    ],
    "stid" : 123434
}

db.students.insertOne(document)


db.students.insertOne({
    "Name" : "Lena",
    "Surname" : "Zrazikova",
    "Age" : 18.0,
    "stid" : 465366
})

document = {
    "Name" : "Timur",
    "Surname" : "Churko",
    "Age" : 19,
    "Group" : {
        "Name" : "KM",
        "Number" : 5,
        "Base" : "Room 341",
        "Head": "Alex Kushnerov"
    },
    "Quotes" : [ 
        "ya takoe ne govoril"
    ],
    "Languages" : ["russian",
    "english"],
    "stid" : 135357
}

db.students.insertOne(document)

db.students.find()

db.students.insertOne({"Name" : "Anna",
    "Surname" : "Yablonskaya",
    "Age" : 19,
    "Group" : {
        "Name" : "KM",
        "Number" : 5,
        "Base" : "Room 341",
        "Head": "Alex Kushnerov"
    },
    "Languages" : ["russian",
    "english", "german"],
    "course":2,
    "stid":247351
})

db.students.insertOne({"Name" : ":Lana",
    "Surname" : "Kiriy",
    "Age" : 20,
    "Group" : {
        "Name" : "Web",
        "Number" : 2,
    },
    "course":2, 
    "stid" : 284623
})

marks = {"Geometry": 10,
    "Algebra": 10,
    "Differential equations": 10,
    "Mathematical analysis" : 10,
    "English": 10
}


db.students.update({"Surname":"Yablonskaya"},{$push:{"marks":marks}})

marks = marks = {"Geometry": 9,
    "Algebra": 10,
    "Differential equations": 8,
    "Mathematical analysis" : 8,
    "English": 8
}
db.students.update({"Surname":"Vasilevski"},{$push:{"marks":marks}})


marks = {"Geometry": 10,
    "Algebra": 9,
    "Mathematical analysis" : 5,
    "English": 8,
    "Web":8
}
db.students.update({"Surname":"Kiriy"},{$push:{"marks":marks}})

marks = {"Geometry": 9,
    "Algebra": 8,
    "Differential equations": 8,
    "Mathematical analysis" : 7,
    "English": 10
}
db.students.update({"Surname":"Churko"},{$push:{"marks":marks}})

marks = {"Geometry": 10,
    "Algebra": 6,
    "Differential equations": 8,
    "Mathematical analysis" : 7,
    "English": 8
}
db.students.update({"Surname":"Zrazikova"},{$push:{"marks":marks}})

db.students.find()

db.students.find({"Age":19})

db.students.find({"marks.Geometry":{$gt:8}})

db.students.aggregate(
[
{
    $group:
    {
        _id:"$Group.Name",
        AverageGeometry: {$avg:"$marks.Geometry"},
        MinimumAlgebra: {$min:"$marks.Algebra"},
        totalAge: {$sum:"$Age"}
    }
}
])

db.students.find({$or :
[
{"marks.Geometry":{$gte:9}},
{"Group.Name" : "Web"}
]}, {"Surname":true})


db.students.find({ "Languages":"russian"}, {"Surname":true,"Languages":true})

db.students.find({ "Languages":{$all:["german","russian"]} }, {"Surname":true,"Languages":true})

critfunc = function(){
    if (this.Age<19)
        {
            return true
        }
        return false
    }
    
db.students.find({$where:critfunc}, {"Surname":true, "marks":true, "Age":true})
  
critfunc = function(){
    if (this.marks.Geometry > 7)
        {
            return true
        }
        return false
    }
    
db.students.find({$where:critfunc}, {"Surname":true, "marks":true, "Age":true})  
   
critfunc = function(){
    if (this.marks.Algebra > 8 || this.Age == 19)
        {
            return true
        }
        return false
    }
    
db.students.find({$where:critfunc}, {"Surname":true, "marks":true, "Age":true})
  
db.students.createIndex({
  "stid":1,  
},
{
      unique: true,
      sparse: true,
      expireAfterSeconds: 3600
  })
  
db.students.find().sort( { "stid": 1 } )
        
