

function addLegg() {
    var numbLegs = document.getElementById('NewShape.LegCount').value;
    document.getElementById(`Shape.Leg${numbLegs}.degreeLabel`).style.display = "unset";
    document.getElementById(`Shape.Leg${numbLegs}.directionLabel`).style.display = "unset"; 
    document.getElementById(`Shape.Leg${numbLegs}.direction`).style.display = "unset";
    document.getElementById(`Shape.Leg${numbLegs}.Mandrel`).style.display = "unset";
    var legName = "";
    switch (numbLegs) {
        case "0":
            legName = "2nd";
            break;
        case "1":
            legName = "3rd";
            break;
        case "2":
            legName = "4th";
            break;
        default:
            legName = (parseInt(numbLegs)+2) + "th";
    }
    numbLegs++;
    legInput = `
<hr/>
        <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
            <div class="panel panel-default">
                <div class="panel-heading" role="tab" id="headingLeg${numbLegs}">
                    <h4 class="panel-title">
                        <a class="collapsed" data-toggle="collapse" data-parent="#accordion" href="#collapseLeg${numbLegs}" aria-expanded="false" aria-controls="collapseLeg${numbLegs}">

                            ${legName} Leg
                                    </a>
                    </h4>
                </div>
                <div id="collapseLeg${numbLegs}" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingLeg${numbLegs}}">
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-4 form-group">
                                <label>
                                    ${legName} Leg Length:
                                            </label>
                                <input class="form-control" name="Legs[${numbLegs}].Length" id="Shape.Leg${numbLegs}.lenght" />
                            </div>
                            <div class="col-4 form-group" style="display:none" id="Shape.Leg${numbLegs}.degreeLabel">
                                <label>
                                    ${legName} Leg Degree:
                                            </label>
                                <input class="form-control" value="" min="0" max="180" type="number" step="any" name="Legs[${numbLegs}].Degree" id="Shape.Leg${numbLegs}.degree" />
                            </div>

                            <div class="col-4 form-group" style="display:none" id="Shape.Leg${numbLegs}.directionLabel">
                                <label>
                                    ${legName} Leg Direction:
                                            </label>
                                <div class="input-group" style="display:none" id="Shape.Leg${numbLegs}.direction">
                                    <div class="input-group-btn" data-toggle="buttons">
                                        <label class="btn btn-info" id="Legs0.IsLeft">
                                            <input type="radio" name="Legs[${numbLegs}].IsRight" value="false" autocomplete="off" hidden>Left
                                                    </label>
                                            <label class="btn btn-info" id="Legs0.IsRight">
                                                <input type="radio" name="Legs[${numbLegs}].IsRight" value="true" autocomplete="off" hidden>Right
                                                    </label>

                                                </div>
                                            </div>
                                    </div>

                                    <div class="row" style="display: none" id="Shape.Leg${numbLegs}.Mandrel">
                                        <div class="col-12 text-center">Mandrel Size</div>
                                        <div class="input-group justify-content-center" style="margin-left: 45px;">
                                            <div class="input-group-btn" data-toggle="buttons">
                                                <label class="btn btn-secondary" id="Shapes.leg${numbLegs}.mandrelNone">
                                                    <input type="radio" name="Legs[${numbLegs}].Mandrel" id="Legs[${numbLegs}].Mandrel.None" value="None" autocomplete="off" hidden>None
                                                 </label>   
                                                    <label class="btn btn-secondary" id="Shapes.leg${numbLegs}.mandrelSmall">
                                                        <input type="radio" name="Legs[${numbLegs}].Mandrel" id="Legs[${numbLegs}].Mandrel.Small" value="Small" autocomplete="off" hidden>Small
                                                    </label>

                                                        <label class="btn btn-secondary" id="Shapes.leg${numbLegs}.mandrelMedium">
                                                            <input type="radio" name="Legs[${numbLegs}].Mandrel" id="Legs[${numbLegs}].Mandrel.Medium" value="Medium" autocomplete="off" hidden>Medium
                                                    </label>

                                                            <label class="btn btn-secondary" id="Shapes.leg${numbLegs}.mandrelLarge">
                                                                <input type="radio" name="Legs[${numbLegs}].Mandrel" value="Large" id="Legs[${numbLegs}].Mandrel.Large" autocomplete="off" hidden>Large
                                                    </label>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>`;
    var newLegDiv = document.getElementById('NewLeg').innerHTML;
    document.getElementById('NewLeg').innerHTML = newLegDiv + legInput;
    document.getElementById('NewShape.LegCount').value = numbLegs;
    validLegCombination();
}

function checkFormFieldsEmpty() {
    var fields =getAllFormValues();
    var i, l = fields.length;
    var fieldname;
    for (i = 0; i < l; i++) {
        fieldname = fields[i];
        if (document.forms["NewShape"][fieldname].value === "" || document.forms["NewShape"][fieldname].value === "0") {
            alert(fieldname + " can not be empty");//testing
            return false;
        }
    }
    return true;
}

function validLegCombination() {
    var form = "NewShape";
    var barSize = document.forms[form]["BarSize"].value;
    var numbLegs = document.getElementById(`${form}.LegCount`).value;
    for (var i = 0; i < numbLegs; i++) {
        var mandrel = document.forms[form][`Legs[${i}].Mandrel`].value;
        var mandrelItem = `Legs[${i}].Mandrel.`;
        switch (barSize) {
            case "3":
                document.getElementById(mandrelItem + "None").disabled = false;
                $(`#Shapes.leg${i}.mandrelNone`).popover("dispose");
                document.getElementById(mandrelItem + "Small").disabled = false;
                $("#" + "Shapes.leg0.mandrelSmall").popover("dispose");
                document.getElementById(mandrelItem + "Medium").disabled = false;
                $("#" + "Shapes.leg0.mandrelMed").popover("dispose");
                break;
            case "4":
                document.getElementById(mandrelItem + "None").disabled = true;
                document.getElementById(mandrelItem + "None").checked = false;
                document.getElementById(`Shapes.leg${i}.mandrelNone`).classList.remove("active");
                $(`#Shapes.leg${i}.mandrelNone`).popover({ content: "Invalid Bar Size for this Mandrel", trigger: "hover", placement: "bottom" }).popover('show');

                document.getElementById(mandrelItem + "Small").disabled = false;
                $(`#Shapes.leg${i}.mandrelSmall`).popover("dispose");
                document.getElementById(mandrelItem + "Medium").disabled = false;
                $(`#Shapes.leg${i}.mandrelMed`).popover("dispose");
                break;
            case "5":
                document.getElementById(`Legs[${i}].Mandrel.None`).disabled = true;
                document.getElementById(`Legs[${i}].Mandrel.None`).checked = false;
                document.getElementById(`Shapes.leg${i}.mandrelNone`).classList.remove("active");
                $(`#Shapes.leg${i}.mandrelNone`).popover({ content: "Invalid Bar Size for this Mandrel", trigger: "hover" });

                document.getElementById(`Legs[${i}].Mandrel.Small`).disabled = true;
                document.getElementById(`Legs[${i}].Mandrel.Small`).checked = false;
                document.getElementById(`Shapes.leg${i}.mandrelSmall`).classList.remove("active");
                $(`#Shapes.leg${i}.mandrelSmall`).popover({ content: "Invalid Bar Size for this Mandrel", trigger: "hover" });

                document.getElementById(`Legs[${i}].Mandrel.Medium`).disabled = false;
                $(`#Shapes.leg${i}.mandrelMed`).popover("dispose");
                break;
            case "6":
                document.getElementById(`Legs[${i}].Mandrel.None`).disabled = true;
                document.getElementById(`Legs[${i}].Mandrel.None`).checked = false;
                document.getElementById(`Shapes.leg${i}.mandrelNone`).classList.remove("active");
                $(`#Shapes.leg${i}.mandrelNone`).popover({ content: "Invalid Bar Size for this Mandrel", trigger: "hover" });

                document.getElementById(`Legs[${i}].Mandrel.Small`).disabled = true;
                document.getElementById(`Legs[${i}].Mandrel.Small`).checked = false;
                document.getElementById(`Shapes.leg${i}.mandrelSmall`).classList.remove("active");
                $(`#Shapes.leg${i}.mandrelSmall`).popover({ content: "Invalid Bar Size for this Mandrel", trigger: "hover" });

                document.getElementById(`Legs[${i}].Mandrel.Medium`).disabled = true;
                document.getElementById(`Legs[${i}].Mandrel.Medium`).checked = false;
                document.getElementById(`Shapes.leg${i}.mandrelMedium`).classList.remove("active");
                $(`#Shapes.leg${i}.mandrelMed`).popover({ content: "Invalid Bar Size for this Mandrel", trigger: "hover" });
                break;
        }
    }
}

function getAllFormValues() {
    var fields = ["Qty", "BarSize", "Legs[0].Length"];//will add more fields as the shape increases
    var numbLegs = document.getElementById('NewShape.LegCount').value;
    if (numbLegs >0) {
        for (var legIndex = 1; legIndex <= numbLegs; ++legIndex) {
            fields.push(`Legs[${(legIndex - 1)}].Degree`,`Legs[${(legIndex - 1)}].IsRight`, `Legs[${(legIndex - 1)}].Mandrel`,`Legs[${legIndex}].Length`);
        }
    }

    return fields;
}

function checkLegLenghts() {
    var numbLegs = document.getElementById('NewShape.LegCount').value;
    var result;
    for (var legIndex = 0; legIndex <= numbLegs; ++legIndex) {
        var LegsLenght = document.getElementById(`Shape.Leg${legIndex}.lenght`).value;
        if (LegsLenght > 240) {
            alert("The leg #" + (legIndex + 1) + " can not be more than 240");//testing
            return false;
        }
    }
    return true; 
}

function checkingCutLenght() {
    fields = getAllFormValues();
    var numbLegs = document.getElementById('NewShape.LegCount').value;

    var newShape = {
        ShapeID: "0",
        BarSize: document.forms["NewShape"]["BarSize"].value,
        LegCount: numbLegs,
        Qty: document.forms["NewShape"]["Qty"].value,
        NumCompleted: "",
        Legs: []
    };

    for (var legIndex = 0; legIndex <= numbLegs; ++legIndex) {
        var leg = {
            Length: document.getElementById(`Shape.Leg${legIndex}.lenght`).value,
            SortOrder: (legIndex + 1),
            Degree: document.getElementById(`Shape.Leg${legIndex}.degree`).value,
            Mandrel: { Name: document.forms["NewShape"][`Legs[${(legIndex)}].Mandrel`].value },
            IsRight: document.forms["NewShape"][`Legs[${(legIndex)}].IsRight`].value
        };
        newShape.Legs.push(leg);
    }


    var quote = {
        q: {}
    };
    quote.q.UseFormulas = "true";
    quote.q.Shapes = [];
    quote.q.Shapes.push(newShape);

    $.ajax({
        type: "POST",
        data: quote,
        url: "/Quotes/CheckIfValidShape",
        dataType: 'json',
        success: function (response) {
            if (!response) {
                alert("This shape cuts to more than 240 inches");

            } else {
                $('#NewShapeForm').submit();
            }
            
        }
    });
    return false;
}

function submitForm() {

    if (checkFormFieldsEmpty() && checkLegLenghts()) {
        if (confirm('Are you sure you want to add this shape?'))
            checkingCutLenght();
    }
}
