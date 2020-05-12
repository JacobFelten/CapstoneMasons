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
    if (legs <= 0 ) {
        document.getElementById("validationShape" + shape).classList.add("bg-danger");
    }
}
function vaidateRebar(RebarNumb) {

}
function deleteShape(ShapeNumb) {
    confirm("Are you sure you want to delete Shape #" + ShapeNumb);

    if ((false)) {//making sure you dont delete the last shape
        alert("You cant delete the only shape");
    } else {
        var r = confirm("Are you sure you want to delete Shape #" + ShapeNumb);
        if (false) {//r==true
            document.getElementById(`quantityShape${ShapeNumb}`).min = -9;
            document.getElementById(`quantityShape${ShapeNumb}`).max = -9;
            document.getElementById(`quantityShape${ShapeNumb}`).value = -9;
            var LegNumb = 1;
            var leg = document.getElementById(`Shape${ShapeNumb}.Leg${LegNumb}.lenght`);
            leg.value = -9;
            while (leg != null) {
                leg.value = -9;
                LegNumb++;
                leg = document.getElementById(`Shape${ShapeNumb}.Leg${LegNumb}.lenght`);
            }

            LegNumb = 1;
            var legDegree = document.getElementById(`Shape${ShapeNumb}.Leg${LegNumb}.degree`);
            while (legDegree != null) {
                legDegree.min = -9;
                legDegree.max = -9;
                legDegree.value = -9;
                LegNumb++;
                legDegree = document.getElementById(`Shape${ShapeNumb}.Leg${LegNumb}.degree`);
            }
            

            document.getElementById(`shape-body${ShapeNumb}`).style.display = "none";
        }
    } 
}

function hideDeletedShapes() {//hide previously deleted shapes to "mitigate" bug
    //while (nextShape !=null)
}
//