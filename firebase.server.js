var Firebase = require('firebase');
var myRootRef = new Firebase('https://phobosdb.firebaseIO.com/');
myRootRef.set("hello world!");