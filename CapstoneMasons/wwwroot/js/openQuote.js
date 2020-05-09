function getShapeNames(id) {
    var shapeName = document.getElementById(`Shape${id}.Title`).innerHTML;
    var shapeModal = document.getElementById(`Shape${id}.Modal`).innerHTML = "Editing " + shapeName;
    //console.log(shapeModal); testing
}