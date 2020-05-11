function getShapeNames(id) {
    var shapeName = document.getElementById(`Shape${id}.Title`).innerHTML;
    var shapeModal = document.getElementById(`Shape${id}.Modal`).innerHTML = "Editing " + shapeName;
    //console.log(shapeModal); testing
}
function SubmitShape(id) {
    confirm('Are you sure you want to change a shape? This cannot be undone');
    var form = document.getElementById(id);
}



