function errorHandler(e) {
    window.alert("API request failed.");
}

function addFields(shapes) {
    var shapesInput ="";
    for (i = 0; i < shapes; i++) {
        shapesInput +=
            `<div class="md-form col-mb-1 form-sm">
            <i class="fas fa-square	 prefix grey-text"></i>
            <input name="LegsInShapes[${i}]" type="number" value="1" min="1" id="Shape${i + 1}" class="form-control form-control-sm validate">
                <label data-error="wrong" data-success="valid" for="form29" class="active">Shape ${i+1} legs</label>
         </div>`;
    }
        document.getElementById("shapes").innerHTML = shapesInput;
}

function getLegsintoShapes() {
    var shapes = [];
    var ShapesCount = parseInt(document.getElementById("ShapesCount").value);
    for (i = 0; i < ShapesCount; i++) {
        shapes.push(parseInt(document.getElementById(`Shape${i + 1}`).value));
    }
    return shapes;
}

$('#CreateQuote').submit(function (e) {
    //e.preventDefault(); // avoid to execute the actual submit of the form.
    var model = {
        Name: document.getElementById("Name").value,
        OrderNum: "",
        Creator: document.getElementById("Creator").value,
        ShapesCount: parseInt(document.getElementById("ShapesCount").value),
        Shapes: getLegsintoShapes(),
        UseFormulas: document.getElementById("checkbox1").checked//need 
    };
    //var data = JSON.stringify(model);
    //$.ajax({
    //    type: "POST",
    //    data: data,
    //    url: url,
    //    contentType: "application/json"
    //}).done(function (res){
    //});

    //return false;
});


$(window).on('load', function () {
    if ($('#ShowPopUp').html() == 'True') {
        $('#createQuote').modal('show');
    }
});

