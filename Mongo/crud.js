use shop

db.colls.updateOne({"Name":"Barbara"},{$set:{"Name":"BarbaraNew"}})
db.materials.updateOne({"Name":true},{$set:{"Cost":40}})
db.colls.updateOne({"Name":"Monica"},{$push:{"Ref":"excellent"}})


db.colls.deleteOne({"Name":"Vera"})

mat3={
    "Name":"wood",
    "Cost":50,
    "Desc":"unknown",
}
db.materials.insertOne(mat3)

db.materials.find();
