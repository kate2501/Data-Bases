use kr3
//1
st1 =  {
    "Name" : "Ivan",
    "Surname" : "Ivanov",
    "Age" : 19,
    "absents":[],
    "marks" :{
        "Geometry": 5,
        "Algebra": 5,
        "DU": 5,
        "Matan" : 5,
        "English": 5
        }
    }
    
st2 =  {
    "Name" : "Petr",
    "Surname" : "Petrov",
    "Age" : 19.0,
    "absents":[],
    "marks" :{
        "Geometry": 6,
        "Algebra": 6,
        "DU": 7,
        "Matan" : 8,
        "English": 9
        }
    }
    
st3 =  {
    "Name" : "Petr",
    "Surname" : "Ivanov",
    "Age" : 18.0,
    "absents":[],
    "marks" :{
        "Geometry": 5,
        "Algebra": 8,
        "DU": 9,
        "Matan" : 8,
        "English": 5
        }
    }
    
st4 =  {
    "Name" : "Sergey",
    "Surname" : "Sergeev",
    "Age" : 20.0,
    "absents":[],
    "marks" :{
        "Geometry": 10,
        "Algebra": 6,
        "DU": 7,
        "Matan" : 5,
        "English": 4
        }
    }

db.students.insertMany([st1,st2,st3, st4])
    

db.students.find({},{"Name":true,"Surname":true,"absents":true})
db.students.find({},{"Name":true,"Surname":true,"marks":true})



db.students.updateMany({},{ $set: {"lucky": false } })

//2

db.students.find()

// количество зачетов и экзаменов
db.students.update({"Surname":"Ivanov"},{ $set: {"credits": 3 } })

db.students.update({"Surname":"Petrov"},{ $set: {"credits": 4 } })

db.students.update({"Surname":"Sergeev"},{ $set: {"credits": 5 } })

//если оценка больше 4 и все сдано
db.students.find().forEach(
  function(m) {
  var array = document.marks;
  var i = 0, sum = 0, len = array.length;
  var num = document.credits;
    while (i < len) {
        sum = sum + array[i++];
    }
   if(sum / len>4 && num == 5){
  db.students.update(document, { $set: {lucky: true } })};
}); 

db.students.find()
