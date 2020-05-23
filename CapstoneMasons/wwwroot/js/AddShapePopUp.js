

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
                                                    <input type="radio" name="Legs[${numbLegs}].Mandrel" value="None" autocomplete="off" hidden>None
                                                    </label>

                                                    <label class="btn btn-secondary" id="Shapes.leg${numbLegs}.mandrelSmall">
                                                        <input type="radio" name="Legs[${numbLegs}].Mandrel" value="Small" autocomplete="off" hidden>Small
                                                    </label>

                                                        <label class="btn btn-secondary" id="Shapes.leg${numbLegs}.mandrelMed">
                                                            <input type="radio" name="Legs[${numbLegs}].Mandrel" value="Medium" autocomplete="off" hidden>Medium
                                                    </label>

                                                            <label class="btn btn-secondary" id="Shapes.leg${numbLegs}.mandrelLarge">
                                                                <input type="radio" name="Legs[${numbLegs}].Mandrel" value="Large" autocomplete="off" hidden>Large
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
}

function checkFormFieldsEmpty() {
    var fields =getAllFormValues();
    var i, l = fields.length;
    var fieldname;
    for (i = 0; i < l; i++) {
        fieldname = fields[i];
        if (document.forms["NewShape"][fieldname].value === "" || document.forms["NewShape"][fieldname].value === "0") {
            alert(fieldname + " can not be empty");//testing
            return true;
        }
    }
    return false;
}

function validLegCombination(formID) {

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

    for (var legIndex = 0; legIndex <= numbLegs; ++legIndex) {
        var LegsLenght = document.getElementById(`Shape.Leg${legIndex}.lenght`).value;
        if (LegsLenght > 240) {
            alert("The leg #" + (legIndex + 1) + " can not be more than 240");//testing
            return true;
        }
    }

    if (numbLegs > 0 && checkingCutLenght()) {//checking for cut lenght
      
    }
    return false;
}

function getMandrel() { //tbd
    return "";//testing
}

function checkingCutLenght() {
    fields = getAllFormValues();
    var numbLegs = document.getElementById('NewShape.LegCount').value;

    var legs = {};
    for (var legIndex = 0; legIndex <= numbLegs; ++legIndex)
    {
        var leg = {
            Length: document.getElementById(`Shape.Leg${legIndex}.lenght`).value,
            SortOrder: legIndex,
            Degree: document.getElementById(`Shape.Leg${legIndex}.lenght`).value,//tbd
            Mandrel: getMandrel(),// var mandrel tbd
            IsRight: ""//tbd
        };
        Object.assign(legs,leg)
    }

    var newShape = {
        ShapeID: "0",
        BarSize: document.forms["NewShape"]["BarSize"].value,
        LegCount: numbLegs,
        Qty: document.forms["NewShape"]["Qty"].value,
        NumCompleted: "",
        Legs: {}// var legs tbd
    };

    var quote = {};
    quote.UseFormulas= "true";
    quote.Shapes = [];
    quote.Shapes.push( newShape);


    var data = JSON.stringify(quote);
    $.ajax({
        type: 'POST',
        data: data,
        url: '/Quotes/CheckIfValidShape',
        dataType: 'json',
        success: function (response) {
            alert("111");
        }
    }).done(function (res){
    });
    return false;
}

function submitForm(event) {
    event.preventDefault();//testing
    if (confirm('Are you sure you want to add this shape?')) {
        if (checkFormFieldsEmpty() || checkLegLenghts(event)) {
                event.preventDefault();
        }
           
        
    }

}