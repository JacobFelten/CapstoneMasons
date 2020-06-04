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
    
    //document.getElementById("validationShape" + shape).classList.remove("bg-danger");"Shape[@(i)].validation"
    ////document.getElementById(`shape${shape}`).classList.add("bg-warning")
    //document.getElementById("validationShape"+shape).classList.add("bg-success");
    if (legs <= 0 ) {
        document.getElementById("validationShape" + shape).classList.add("bg-danger");
    }
}

function validLegCombination(shape) {
    var barSize = document.querySelector('input[name="'+shape + '.BarSize"]:checked').value;
    var numbLegs = document.getElementById(`${shape}.LegCount`).value;
    for (var i = 0; i < numbLegs; i++) {
        var mandrelItem = `${shape}.leg[${i}].Mandrel.`;
        switch (barSize) {
            case "3":
                document.getElementById(mandrelItem + "None").disabled = false;//every mandrel is available
                document.getElementById(mandrelItem + "None").checked = true;//preselect lowest mandrel
                document.getElementById(mandrelItem + "None.Label").classList.add("active");
                $("#" + mandrelItem + "None.Label").tooltip("hide");

                document.getElementById(mandrelItem + "Small").disabled = false;
                document.getElementById(mandrelItem + "Small").checked = false;
                document.getElementById(mandrelItem + "Small.Label").classList.remove("active");
                $("#" + mandrelItem + "Small.Label").popover("hide");


                document.getElementById(mandrelItem + "Medium").disabled = false;
                document.getElementById(mandrelItem + "Medium").checked = false;
                document.getElementById(mandrelItem + "Medium.Label").classList.remove("active");
                $("#" + mandrelItem + "Medium.Label").popover("hide");

                document.getElementById(mandrelItem + "Large").checked = false;
                document.getElementById(mandrelItem + "Large.Label").classList.remove("active");
                break;
            case "4":
                document.getElementById(mandrelItem + "None").disabled = true;//small and med and large available
                document.getElementById(mandrelItem + "None").checked = false;//small preselected
                document.getElementById(mandrelItem + "None.Label").classList.remove("active");
                $("#" + mandrelItem + "None.Label").tooltip({ title: "Invalid Bar Size for this Mandrel", trigger: "hover", placement: "bottom" });
                $("#" + mandrelItem + "None.Label").tooltip();

                document.getElementById(mandrelItem + "Small").disabled = false;
                document.getElementById(mandrelItem + "Small").checked = true;
                document.getElementById(mandrelItem + "Small.Label").classList.add("active");
                $("#" + mandrelItem + "Small.Label").popover("hide");

                document.getElementById(mandrelItem + "Medium").disabled = false;
                document.getElementById(mandrelItem + "Medium").checked = false;
                document.getElementById(mandrelItem + "Medium.Label").classList.remove("active");
                $("#" + mandrelItem + "Medium.Label").tooltip("hide");

                document.getElementById(mandrelItem + "Large").checked = false;
                document.getElementById(mandrelItem + "Large.Label").classList.remove("active");
                break;
            case "5":
                document.getElementById(mandrelItem + "None").disabled = true;
                document.getElementById(mandrelItem + "None").checked = false;
                document.getElementById(mandrelItem + "None.Label").classList.remove("active");
                $("#" + mandrelItem + "None.Label").popover({ content: "Invalid Bar Size for this Mandrel", trigger: "hover", placement: "bottom" });

                document.getElementById(mandrelItem + "Small").disabled = true;
                document.getElementById(mandrelItem + "Small").checked = false;
                document.getElementById(mandrelItem + "Small.Label").classList.remove("active");
                $("#" + mandrelItem + "Small.Label").popover({ content: "Invalid Bar Size for this Mandrel", trigger: "hover", placement: "bottom" });

                document.getElementById(mandrelItem + "Medium").disabled = false;//med and large available
                document.getElementById(mandrelItem + "Medium").checked = true;//medium preselected
                document.getElementById(mandrelItem + "Medium.Label").classList.add("active");
                $("#" + mandrelItem + "Medium.Label").popover("hide");

                document.getElementById(mandrelItem + "Large").checked = false;
                document.getElementById(mandrelItem + "Large.Label").classList.remove("active");
                break;
            case "6":
                document.getElementById(mandrelItem + "None").disabled = true;
                document.getElementById(mandrelItem + "None").checked = false;
                document.getElementById(mandrelItem + "None.Label").classList.remove("active");
                $("#" + mandrelItem + "None.Label").popover({ content: "Invalid Bar Size for this Mandrel", trigger: "hover", placement: "bottom" });

                document.getElementById(mandrelItem + "Small").disabled = true;
                document.getElementById(mandrelItem + "Small").checked = false;
                document.getElementById(mandrelItem + "Small.Label").classList.remove("active");
                $("#" + mandrelItem + "Small.Label").popover({ content: "Invalid Bar Size for this Mandrel", trigger: "hover", placement: "bottom" });

                document.getElementById(mandrelItem + "Medium").disabled = true;
                document.getElementById(mandrelItem + "Medium").checked = false;
                document.getElementById(mandrelItem + "Medium.Label").classList.remove("active");
                $("#" + mandrelItem + "Medium.Label").popover({ content: "Invalid Bar Size for this Mandrel", trigger: "hover", placement: "bottom" });

                document.getElementById(mandrelItem + "Large").checked = true;//large available and preselected
                document.getElementById(mandrelItem + "Large.Label").classList.add("active");
                break;
        }
    }
}

function checkFormFields(shape) {
    classList = document.getElementById(shape + '.Validation').classList;
    if (classList.contains('bg-warning')) {
        document.getElementById(shape + '.Validation').classList.remove("bg-warning");
    } else if (classList.contains('bg-success')) {
        document.getElementById(shape + '.Validation').classList.remove("bg-success");
    }
    document.getElementById(shape + '.Outline').style.outline = "";
    var fields = getAllFormValues(shape);
    var i, l = fields.length;
    var fieldname;
    var result = ""
    for (i = 0; i < l && result.length == 0; i++) {
        fieldname = shape+fields[i];
        if (document.forms['CreateQuote'][fieldname].value === "" ||
            document.forms['CreateQuote'][fieldname].value === "0") {
            //show invalid shape when empty
            result = fields[i] + " can not be empty";
            

        } else if (document.forms['CreateQuote'][fieldname].value <= -1) {
            //show invalid shape when negative
            result = fields[i] + " can not be negative";
        }
    }
    if (result.length > 0) {
        document.getElementById(shape + '.Validation').classList.add("bg-warning");
        document.getElementById(shape + '.Validation').innerHTML = result; //styling changes for a invalid shape
        document.getElementById(shape + '.Outline').style.outline = "6px solid #ffc107";
        document.getElementById('CreateQuote.Submit').disabled = true;
    } else {
        document.getElementById(shape + '.Validation').classList.add("bg-success");
        document.getElementById(shape + '.Validation').innerHTML = "Valid Shape";//styling changes for a valid shape
        document.getElementById(shape + '.Outline').style.outline = "6px solid #28a745";
        document.getElementById('CreateQuote.Submit').disabled = false;
    }

}

function getAllFormValues(shape) {
    var fields = [".Qty", ".BarSize", ".Legs[0].Length"];//will add more fields as the shape increases
    var numbLegs = document.getElementById(shape + '.LegCount').value;
    if (numbLegs > 0) {
        for (var legIndex = 1; legIndex <= numbLegs; ++legIndex) {
            fields.push(`.Legs[${(legIndex - 1)}].Degree`, `.Legs[${(legIndex - 1)}].IsRight`, `.Legs[${(legIndex - 1)}].Mandrel.Name`, `.Legs[${legIndex}].Length`);
        }
    }

    return fields;
}

function deleteShape(ShapeNumb) {
    var numbShapes = document.getElementById(`Shapes.Count`);
    

    if (numbShapes.value < 2) {//making sure you dont delete the last shape
        alert("You cant delete the only shape");
    } else {
        ShapeNumb;
        var r = confirm("Are you sure you want to delete Shape #" + (ShapeNumb));
        if (r == true) {
            ShapeNumb;
            numbShapes.value = numbShapes.value -1;//setting bogus values to know which shape to delete
            document.getElementById(`quantityShape${ShapeNumb}`).min = -9;
            document.getElementById(`quantityShape${ShapeNumb}`).max = -9;
            document.getElementById(`quantityShape${ShapeNumb}`).value = -9;
            var LegNumb = 1;
            var legLenght = document.getElementById(`Shape${ShapeNumb}.Leg${LegNumb}.lenght`);
            while (legLenght != null) {
                legLenght.value = -9;
                legLenght.min = -9;
                legLenght.max = -9;
                LegNumb++;
                legLenght = document.getElementById(`Shape${ShapeNumb}.Leg${LegNumb}.lenght`);
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

function hideDeletedShapes() {//hide previously deleted shapes to "mitigate" bug when going back

    var numbShapes = document.getElementById(`Shapes.Count`);
    for (var ShapeNumb = 1; ShapeNumb < numbShapes; i++) {
        if (document.getElementById(`quantityShape${ShapeNumb}`).value == -9
            && document.getElementById(`Shape${ShapeNumb}.Leg${ShapeNumb}.lenght`).value == -9)
            document.getElementById(`shape-body${ShapeNumb}`).style.display = none;
            document.getElementById(`quantityShape${ShapeNumb}`).min = -9;//rehide and make shape marked to delete
            document.getElementById(`quantityShape${ShapeNumb}`).max = -9;

            var LegNumb = 1;
            var legLenght = document.getElementById(`Shape${ShapeNumb}.Leg${LegNumb}.lenght`);
            while (legLenght != null) {
                legLenght.value = -9;
                legLenght.min = -9;
                legLenght.max = -9;
                LegNumb++;
                legLenght = document.getElementById(`Shape${ShapeNumb}.Leg${LegNumb}.lenght`);
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
        }
}
//