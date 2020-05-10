function getShapeNames(id) {
    var shapeName = document.getElementById(`Shape${id}.Title`).innerHTML;
    var shapeModal = document.getElementById(`Shape${id}.Modal`).innerHTML = "Editing " + shapeName;
    //console.log(shapeModal); testing
}

var form = document.getElementById('SubmitShape');

form.addEventListener('submit', function () {
    console.log('invoked');
    var formData = new FormData(form);

    //var data = JSON.stringify(prices);
    $.ajax({
        type: "POST",
        data: formData,
        url: "/Quotes/EditShapeAsync",
        //contentType: "application/json"
        dataType: "text",
        success: function (response) {
            alert(response);
        },
        error: function () {

        }
    });
    return false;
});