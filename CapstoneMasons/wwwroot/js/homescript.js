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

function getLegsintoShapes() {//gets legs from shape numbers
    var shapes = [];
    var ShapesCount = parseInt(document.getElementById("ShapesCount").value);
    for (i = 0; i < ShapesCount; i++) {
        shapes.push(parseInt(document.getElementById(`Shape${i + 1}`).value));
    }
    return shapes;
}
$(window).on('shown.bs.modal', function () {
    getLegPrices();
});

function PostPrices() {//sends price in popup to pst method
    var prices = {
        bar3GlobalID: document.getElementById("bar3costID").value,
        bar3GlobalCost: document.getElementById("bar3cost").value,
        bend3GlobalID: document.getElementById("bar3bendID").value,
        bar3BendCost: document.getElementById("bar3bend").value,
        cut3GlobalID: document.getElementById("bar3cutID").value,
        bar3CutCost: document.getElementById("bar3cut").value,
        bar4GlobalID: document.getElementById("bar4costID").value,
        bar4GlobalCost: document.getElementById("bar4cost").value,
        bend4GlobalID: document.getElementById("bar4bendID").value,
        bar4BendCost: document.getElementById("bar4bend").value,
        cut4GlobalID: document.getElementById("bar4cutID").value,
        bar4CutCost: document.getElementById("bar4cut").value, 
        bar5GlobalID: document.getElementById("bar5costID").value,
        bar5GlobalCost: document.getElementById("bar5cost").value,
        bend5GlobalID: document.getElementById("bar5bendID").value,
        bar5BendCost: document.getElementById("bar5bend").value,
        cut5GlobalID: document.getElementById("bar5cutID").value,
        bar5CutCost: document.getElementById("bar5cut").value,
        bar6GlobalID: document.getElementById("bar6costID").value,
        bar6GlobalCost: document.getElementById("bar6cost").value,
        bend6GlobalID: document.getElementById("bar6bendID").value,
        bar6BendCost: document.getElementById("bar6bend").value,
        cut6GlobalID: document.getElementById("bar6cutID").value,
        bar6CutCost: document.getElementById("bar6cut").value,
        setupGlobalID: document.getElementById("setupGlobalID").value,
        setupCharge: document.getElementById("setupCharge").value,
        minOrderGlobalID: document.getElementById("minOrderGlobalID").value,
        minimumOrderCost: document.getElementById("minimumOrderCost").value
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
        getLegPrices();
    }
});

function getLegPrices() {
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
            document.getElementById("bar4costID").value = response.bar4GlobalCost.costID;
            document.getElementById("bar4cost").value = response.bar4GlobalCost.price;
            document.getElementById("bar5costID").value = response.bar5GlobalCost.costID;
            document.getElementById("bar5cost").value = response.bar5GlobalCost.price;
            document.getElementById("bar6costID").value = response.bar6GlobalCost.costID;
            document.getElementById("bar6cost").value = response.bar6GlobalCost.price;

            document.getElementById("bar3bendID").value = response.bar3BendCost.costID;
            document.getElementById("bar3bend").value = response.bar3BendCost.price;
            document.getElementById("bar4bendID").value = response.bar4BendCost.costID;
            document.getElementById("bar4bend").value = response.bar4BendCost.price;
            document.getElementById("bar5bendID").value = response.bar5BendCost.costID;
            document.getElementById("bar5bend").value = response.bar5BendCost.price;
            document.getElementById("bar6bendID").value = response.bar6BendCost.costID;
            document.getElementById("bar6bend").value = response.bar6BendCost.price;

            document.getElementById("bar3bendID").value = response.bar3BendCost.costID;
            document.getElementById("bar3bend").value = response.bar3BendCost.price;
            document.getElementById("bar4bendID").value = response.bar4BendCost.costID;
            document.getElementById("bar4bend").value = response.bar4BendCost.price;
            document.getElementById("bar5bendID").value = response.bar5BendCost.costID;
            document.getElementById("bar5bend").value = response.bar5BendCost.price;
            document.getElementById("bar6bendID").value = response.bar6BendCost.costID;
            document.getElementById("bar6bend").value = response.bar6BendCost.price;

            document.getElementById("bar3cutID").value = response.bar3CutCost.costID;
            document.getElementById("bar3cut").value = response.bar3CutCost.price;
            document.getElementById("bar4cutID").value = response.bar4CutCost.costID;
            document.getElementById("bar4cut").value = response.bar4CutCost.price;
            document.getElementById("bar5cutID").value = response.bar5CutCost.costID;
            document.getElementById("bar5cut").value = response.bar5CutCost.price;
            document.getElementById("bar6cutID").value = response.bar6CutCost.costID;
            document.getElementById("bar6cut").value = response.bar6CutCost.price;

            document.getElementById("setupGlobalID").value = response.bar6CutCost.costID;
            document.getElementById("setupCharge").value = response.bar6CutCost.price;
        },
        error: function () {
            alert("Error Getting Prices");
        }

    });
}

function hideBarCosts(event) {
    
    var classes = document.getElementById("collapse-bar-prices").classList;
    if (classes.contains("show"))
    {
        event.stopPropagation();
        document.getElementById("collapse-bar-prices").classList.remove("show")
    }
}

function alertPricesBanner() {
    var currentTime = new Date().toLocaleString("en-US", { timeZone: "America/Los_Angeles" });
    currentTime = new Date(currentTime.toString());
    var showBanner = false;
    $.ajax({
        type: "GET",
        url: pricesUrl,
        //contentType: "application/json"
        dataType: "text",
        success: function (response)
        {
            response = JSON.parse(response);
            for (var data in response) {
                var date = new Date(response[data].lastChanged);
                var diff = (currentTime - date) / 1000;
                diff /= (60 * 60 * 24 * 7);
                var numPastWeeks = Math.abs(Math.round(diff));
                if (numPastWeeks >= 2) {
                    //"5/7/2020, 4:39:30 PM"  "2020-05-06T15:11:06.7879818"
                    showBanner = true;
                }
            }
            if (!showBanner) {
                document.getElementById("barPricesBanner").style.display = "none";
            }
        }
    });

}