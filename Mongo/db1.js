use shop1



sup1=

{

    "Name":"Pinskdrev",

    "Address":"Pinsk",

    "Manager":"Ivanov",

    "St_date": new ISODate("2010-07-20T18:00:00Z"),

    "End_date": new ISODate("2020-07-20T18:00:00Z")

}



sup2=

{

    "Name":"Mogilevfabric",

    "Address":"Mogilev",

    "Manager":"Petrov",

    "St_date": new ISODate("2010-07-20T18:00:00Z"),

    "End_date": new ISODate("2020-07-20T18:00:00Z")

}



db.suppliers.insertMany([sup1,sup2])

sup1 = db.suppliers.findOne(sup1)

sup2 = db.suppliers.findOne(sup2)



mat1={

    "Name":"wood",

    "Cost":50,

    "Desc":
        {"wood":"birch",
        "color":"light"},

    "Supplier":new DBRef("suppliers", sup1._id)
}



mat2={

    "Name":"leather",

    "Cost":50,

    "Desc": 
        {"leather":"cow",
        "color":"black"},

    "Supplier":new DBRef("suppliers", sup2._id)

}


item1 = {

    "Name":"Chair",

    "Type":"furniture",

    "Desc":"very good",

    "Materials":mat1,

    "Cost":50

}



item2=

{

    "Name":"Bed",

    "Type":"furniture",

    "Desc":"comfortable",

    "Material": [mat1, mat2],

    "Cost":500

}




document=

{

    "Start_date":Date(),

    "End_date": Date("2025-05-18T16:00:00Z"),

    "Name":"Barbara",

    "Items":item1

}



document1=

{

    "Start_date":Date(),

    "End_date": Date("2023-05-18T16:00:00Z"),

    "Name":"Monica",

    "Items":item2

}



db.colls.insertMany([document,document1])



db.colls.find()


db.colls.find({"Items.Name":"Chair"})

db.colls.find({"Items.Materials.Name":"wood"}).limit(1)

db.colls.find({$or:[{"Items.Cost":{$gte:200}},
{"Items.Name":"Chair"}]},
{"Items.Desc":true}).limit(2)

db.colls.find({"Items.Materials.Name":{$all:["wood","leather"]} }, {"Items.Materials.Cost":true})

critfunc = function(){
    if (this.Items.Cost<=50)
        {
            return true
        }
        return false
    }
    
db.colls.find({$where:critfunc})
  
critfunc = function(){
    if (this.Items.Cost<=50  || this.Items.Cost>=300)
        {
            return true
        }
        return false
    }
    
db.colls.find({$where:critfunc})

db.colls.aggregate(
[
{
    $group:
    {
        _id:"$Name",
        count: {$sum:1}
    }
},{ $sort: { count: -1 }}
])

db.colls.aggregate(
[
{
    $group:
    {
        _id:"$Name",
        count: {$avg:"$Items.Cost"}
    }
}
])

db.colls.aggregate(
[
{
    $group:
    {
        _id:"$Name",
        min: {$min:"$Items.Cost"},
        max:{$max:"$Items.Cost"}
    }
}
])

db.colls.aggregate(
[
{$match:{"Items.Name":"Chair"}  }
])

db.colls.aggregate(
[
{$match:{ $and: 
    [
    {"Items.Type": "furniture"} ,
    {"Items.Cost": {$gte:5}}
    ]}}
])
    
