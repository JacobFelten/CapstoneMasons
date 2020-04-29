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
$(window).on('shown.bs.modal', function () {
    $.ajax({
        type: "GET",
        url: pricesUrl,
        //contentType: "application/json"
        dataType: "text",
        success: function (response) {
            //alert(response);
            response = JSON.parse(response);
            document.getElementById("bar3costID").value = response.bar3GlobalCost.costID;
            document.getElementById("bar3cost").value = response.bar3GlobalCost.price;
        },
        error: function () {
            alert("Error Getting Prices");
        }
    });

    var bar3cost = 0;
});

function PostPrices() {
    var prices = {
        bar3GlobalID: document.getElementById("bar3costID").value,
        bar3GlobalCost: document.getElementById("bar3cost").value,
        //bend3GlobalID: 1,
        //bar3BendCost: 1,
        //cut3GlobalID: 1,
        //bar3CutCost: 1,
        //bar4GlobalID: document.getElementById("bar4costID").value,
        //bar4GlobalCost: document.getElementById("bar4cost").value,
        //bend4GlobalID: 1,
        //bar4BendCost: 1,
        //cut4GlobalID: 1,
        //bar4CutCost: 1,
        //bar5GlobalID: 1,
        //bar5GlobalCost: 1,
        //bend5GlobalID: 1,
        //bar5BendCost: 1,
        //cut5GlobalID: 1,
        //bar5CutCost: 1,
        //bar6GlobalID: 1,
        //bar6GlobalCost: 1,
        //bend6GlobalID: 1,
        //bar6BendCost: 1,
        //cut6GlobalID: 1,
        //bar6CutCost: 1,
        //setupGlobalID: 1,
        //setupCharge: 1,
        //minOrderGlobalID: 1,
        //minimumOrderCost: 1
    };
    var data = JSON.stringify(prices);
    $.ajax({
        type: "POST",
        data: prices,
        url: pricesUrl,
        //contentType: "application/json"
        dataType: "text",
        success: function (response) {
            alert(response);
        },
        error: function () {

        }
    });

    return false;
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

