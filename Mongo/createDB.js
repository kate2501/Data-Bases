use shop

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
    "Desc":"birch",
    "Supplier":new DBRef("suppliers", sup1._id),
}

mat2={
    "Name":"leather",
    "Cost":50,
    "Desc":"birch",
    "Supplier":new DBRef("suppliers", sup2._id)
}

db.materials.insertMany([mat1,mat2])
mat1 = db.materials.findOne(mat1)
mat2 = db.materials.findOne(mat2)

item1 = {
    "Name":"Chair",
    "Type":"furniture",
    "Desc":"very good",
    "Material": new DBRef("materials", mat1._id),
    "Cost":50
}

item2=
{
    "Name":"Bed",
    "Type":"furniture",
    "Desc":"comfortable",
    "Material": [new DBRef("materials", mat1._id),new DBRef("materials", mat2._id)],
    "Cost":500
}

db.items.insertMany([item1,item2])
item1 = db.items.findOne(item1)
item2 = db.items.findOne(item2)

document=
{
    "Start_date":Date(),
    "End_date": Date("2025-05-18T16:00:00Z"),
    "Name":"Barbara",
    "Items":new DBRef("items", item1._id)
}

document1=
{
    "Start_date":Date(),
    "End_date": Date("2023-05-18T16:00:00Z"),
    "Name":"Monica",
    "Items":new DBRef("items", item2._id)
}

db.colls.insertMany([document,document1])

db.suppliers.find()

var file = cat('/NG/mdb.json');
var o = JSON.parse(file);
db.suppliers.insert(o)
   
db.suppliers.find()