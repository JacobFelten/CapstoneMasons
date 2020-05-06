//Remove href from shipping and payment from start
//$("#panel-shipping-heading").removeAttr("href").css('color', 'gray')
//$("#panel-payment-heading").removeAttr("href").css('color', 'gray')

//after delivery is shown, add href

//after payment is shown, add href
$('#collapse-payment').on('show.bs.collapse', function () {
    $("#panel-payment-heading").css('color', 'black')//.attr("href", "#collapse-payment")
})

function validateShapes(shape, legs) {
    //var RebarNum = document.getElementById("RebarNum").innerHTML.options;
    
    //document.getElementById("validationShape" + shape).classList.remove("bg-danger");
    ////document.getElementById(`shape${shape}`).classList.add("bg-warning")
    //document.getElementById("validationShape"+shape).classList.add("bg-success");
    if (legs <= 0 || RebarNum.selectedIndex) {
        document.getElementById("validationShape" + shape).classList.add("bg-danger");
    }
}
function vaidateRebar(RebarNumb) {

}
function deleteShape(ShapeNumb) {
    var r = confirm("Are you sure you want to deletea Shape");
    if (r == true) {
        var shape = document.getElementById("shape-body" + ShapeNumb);
        if (document.getElementById("shape-body" + ShapeNumb + 1) != null)
        {

        }
        shape.remove();
    } else {
        
    }
}
//