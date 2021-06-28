use shop

db.suppliers.find()

     
db.items.find({$or:[{"Name":{$regex:"^w"}},{"Desc":{$regex:"^c"}}]})


db.materials.find({$and:[{"Cost":{$lte:800}},
{"Name":"leather"}]},
{"Desc":true}).limit(2)

func = function(){
    if (this.Cost<200  || this.Cost>800)
        {
            return true
        }
        return false
    }
    
    
func1 = function(){
    if (this.Cost<=50)
        {
            return true
        }
        return false
    }    
    
db.items.find(func)
    
db.materials.find(func1)  
    
db.colls.find({"End_date": {$ne: new Date()}})

db.colls.aggregate(
[
{
    $group:
    {
        _id:"$Name",
        cItems: {$sum:1}
    }
}
])


db.materials.aggregate([
{
    $project: { 
            Name: "$Name",
            Cost: "$Cost",
            Desc: "$Desc",
            Supplier: {$arrayElemAt:[{$objectToArray: "$Supplier"}, 1]}
            
}},
{ $project: { 
            Name: "$Name",
            Cost: "$Cost",
            Desc: "$Desc",
            gSupplier: "$Supplier.v"}
            },
            
  {
     $lookup: {
        from: "suppliers",
        localField: "gSupplier",
        foreignField: "_id",
        as: "SupplierInfo"
        }
    },
    {
        $project: {
             Name: "$Name",
            Cost: "$Cost",
            Desc: "$Desc",
            SupplierInfo: "$SupplierInfo"},
            }              
])
            

db.colls.aggregate([
{
    $project: { 
            Name: "$Name",
            End_date: "$End_date",
            Start_date: "$Start_date",
            Items: {$arrayElemAt:[{$objectToArray: "$Items"}, 1]}
            
}},
{ $project: { 
            Name: "$Name",
            End_date: "$End_date",
            Start_date: "$Start_date",
            gItems: "$Items.v"}
            },
            
  {
     $lookup: {
        from: "items",
        localField: "gItems",
        foreignField: "_id",
        as: "ItemsInfo"
        }
    },
    {
        $project: {
             Name: "$Name",
            Cost: "$Cost",
            Desc: "$Desc",
            ItemsInfo: "$ItemsInfo"},
            },
    {
     $group:{
        _id:"$Name",
        min: {$min:"$ItemsInfo.Cost"},
        max: {$min:"$ItemsInfo.Cost"},
        total:{$sum:1}
    }
                }
])


db.materials.aggregate(
[
{$match:{ $and: 
    [
    {Name: "wood"} ,
    {Cost: {$gte:5}}
    ]}}
])
    
db.materials.aggregate([
{
    $project: { 
            Name: "$Name",
            Cost: "$Cost",
            Desc: "$Desc",
            Supplier: {$arrayElemAt:[{$objectToArray: "$Supplier"}, 1]}
            
}},
{ $project: { 
            Name: "$Name",
            Cost: "$Cost",
            Desc: "$Desc",
            gSupplier: "$Supplier.v"}
            },
            
  {
     $lookup: {
        from: "suppliers",
        localField: "gSupplier",
        foreignField: "_id",
        as: "SupplierInfo"
        }
    },
    {
        $project: {
            Name: "$Name",
            Cost: "$Cost",
            Desc: "$Desc",
            SupplierInfo: "$SupplierInfo"},
            },
{$match:{Address:"Pinsk"}  }         
])


db.materials.aggregate([{
    $project:
        {
            _id: "$Name",
            res: {
                $cond: {
                if: func1,
                then: "More than 50",
                else: "No"
            }
        }
    }
}])