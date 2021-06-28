use shop

db.items.find({"Cost":{$gte:50}}).forEach(function(m)
{
    m.Cost-=49
}
)
    

func = function(name){
    var doc = db.items.findOne({"Name":name}); 
    var doc1= db.items.findOne({"Name":{$ne:name}}); 
    var uns = doc.Cost/2;
    doc.Cost -= uns;
    doc1.Cost += uns;
    db.items.save(doc);
    db.items.save(doc)}
   
db.eval(func("Chair"))
db.items.find()