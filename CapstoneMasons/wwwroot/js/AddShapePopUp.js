

function addLegg(form) {
    var numbLegs = document.getElementById(`${form}.LegCount`).value;
    document.getElementById(`${form}.leg[${numbLegs}].degreeLabel`).style.display = "unset";
    document.getElementById(`${form}.leg[${numbLegs}].directionLabel`).style.display = "unset"; 
    document.getElementById(`${form}.leg[${numbLegs}].direction`).style.display = "unset";
    document.getElementById(`${form}.leg[${numbLegs}].Mandrel`).style.display = "unset";
    document.getElementById(`${form}.leg[${numbLegs}].delete`).style.display = "none";
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
        <div class="panel-group" id="${form}.leg[${numbLegs}].accordion" role="tablist" aria-multiselectable="true">
            <div class="panel panel-default"><input name="Legs[${numbLegs}].SortOrder" value="${numbLegs}" hidden/>
                <div class="panel-heading" role="tab" id="headingLeg${numbLegs}">
                    <h4 class="panel-title">
                        <a class="collapsed" data-toggle="collapse" data-parent="#accordion" href="#collapseLeg${numbLegs}" aria-expanded="false" aria-controls="collapseLeg${numbLegs}">

                            ${legName} Leg
                                    </a>
                                                <i id="NewShape.leg[${numbLegs}].delete" class="fas fa-trash-alt float-right hidden" style="text-shadow: 0 0 3px #000;font-size: 1em;color: tomato;margin-top: 5px;margin-right: 15px;" onclick="deleteLeg('${form}.leg[${numbLegs}].accordion','NewShape')"></i>
                    </h4>
                </div>
                <div id="collapseLeg${numbLegs}" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingLeg${numbLegs}">
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-4 form-group">
                                <label>
                                    ${legName} Leg Length:
                                            </label>
                                <input class="form-control" name="Legs[${numbLegs}].Length" id="NewShape.leg[${numbLegs}].lenght" />
                            </div>
                            <div class="col-4 form-group" style="display:none" id="NewShape.leg[${numbLegs}].degreeLabel">
                                <label>
                                    ${legName} Leg Degree:
                                            </label>
                                <input class="form-control" value="" min="0" max="180" type="number" step="any" name="Legs[${numbLegs}].Degree" id="NewShape.leg[${numbLegs}].degree" />
                            </div>

                            <div class="col-4 form-group" style="display:none" id="NewShape.leg[${numbLegs}].directionLabel">
                                <label>
                                    ${legName} Leg Direction:
                                            </label>
                                <div class="input-group" style="display:none" id="NewShape.leg[${numbLegs}].direction">
                                    <div class="input-group-btn" data-toggle="buttons">
                                        <label class="btn btn-info" id="NewShape.leg[${numbLegs}].IsLeft">
                                            <input type="radio" name="Legs[${numbLegs}].IsRight" value="false" autocomplete="off" hidden>Left
                                                    </label>
                                            <label class="btn btn-info" id="NewShape.leg[${numbLegs}].IsRight">
                                                <input type="radio" name="Legs[${numbLegs}].IsRight" value="true" autocomplete="off" hidden>Right
                                                    </label>

                                                </div>
                                            </div>
                                    </div>

                                    <div class="row" style="display: none" id="NewShape.leg[${numbLegs}].Mandrel">
                                        <div class="col-12 text-center">Mandrel Size</div>
                                        <div class="input-group justify-content-center" style="margin-left: 45px;">
                                            <div class="input-group-btn" data-toggle="buttons">
                                                <label class="btn btn-secondary" id="NewShape.leg[${numbLegs}].Mandrel.None.Label">
                                                    <input type="radio" name="Legs[${numbLegs}].Mandrel" id="NewShape.leg[${numbLegs}].Mandrel.None" value="None" autocomplete="off" hidden>None
                                                 </label>   
                                                    <label class="btn btn-secondary" id="NewShape.leg[${numbLegs}].Mandrel.Small.Label">
                                                        <input type="radio" name="Legs[${numbLegs}].Mandrel" id="NewShape.leg[${numbLegs}].Mandrel.Small" value="Small" autocomplete="off" hidden>Small
                                                    </label>

                                                        <label class="btn btn-secondary" id="NewShape.leg[${numbLegs}].Mandrel.Medium.Label">
                                                            <input type="radio" name="Legs[${numbLegs}].Mandrel" id="NewShape.leg[${numbLegs}].Mandrel.Medium" value="Medium" autocomplete="off" hidden>Medium
                                                    </label>

                                                            <label class="btn btn-secondary" id="NewShape.leg[${numbLegs}].Mandrel.Large.Label">
                                                                <input type="radio" name="Legs[${numbLegs}].Mandrel" value="Large" id="NewShape.leg[${numbLegs}].Mandrel.Large" autocomplete="off" hidden>Large
                                                    </label>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>`;
    var newLegDiv = document.getElementById(form+'.NewLeg').innerHTML;
    document.getElementById(form+'.NewLeg').innerHTML = newLegDiv + legInput;
    document.getElementById(form+'.LegCount').value = numbLegs;
    validLegCombination(form);
}

function checkFormFieldsEmpty(form) {
    var fields =getAllFormValues(form);
    var i, l = fields.length;
    var fieldname;
    for (i = 0; i < l; i++) {
        fieldname = fields[i];


        //show invalid shape when empty

        //nicer validation message
        if (fields[i] == 'Qty') {
            fields[i] = 'The Quantity'
        } else if (fields[i].substring(0, 4) == 'Legs') {
            fields[i] = 'Leg #' + fields[i].substring(5); //nicer validation
            var legIndex = (parseInt(fields[i].charAt(5)) + 1).toString();
            if (fields[i].slice(-6) == 'Degree') {
                fields[i] = 'Degrees in ' + fields[i].substring(0, 5) + legIndex;
            } else if (fields[i].slice(-6) == 'l.Name') {
                fields[i] = 'Mandrels in ' + fields[i].substring(0, 5) + legIndex;
            } else if (fields[i].slice(-6) == 'sRight') {
                fields[i] = 'The angle in ' + fields[i].substring(0, 5) + legIndex;
            } else if (fields[i].slice(-6) == 'Length') {
                fields[i] = 'The length in ' + fields[i].substring(0, 5) + legIndex;
            }
        }

        if (document.forms[form][fieldname].value === "" || document.forms[form][fieldname].value === "0") {


            alert(fields[i] + " can not be empty");//testing
            return false;
        } else if (document.forms[form][fieldname].value <= -1) {
            alert(fields[i] + " can not be negative");//testing
            return false;
        }
    }
    return true;
}

function validLegCombination(form) {
    var barSize = document.forms[form]["BarSize"].value;
    var numbLegs = document.getElementById(`${form}.LegCount`).value;
    for (var i = 0; i < numbLegs; i++) {
        var mandrelItem = `${form}.leg[${i}].Mandrel.`;
        switch (barSize) {
            case "3":
                document.getElementById(mandrelItem + "None").disabled = false;//every mandrel is available
                $("#"+mandrelItem+"None.Label").popover("hide");
                document.getElementById(mandrelItem + "Small").disabled = false;
                $("#" + mandrelItem +"Small.Label").popover("hide");
                document.getElementById(mandrelItem + "Medium").disabled = false;
                $("#" + mandrelItem + "Small.Label").popover("hide");
                break;
            case "4":
                document.getElementById(mandrelItem + "None").disabled = true;
                document.getElementById(mandrelItem + "None").checked = false;
                document.getElementById(mandrelItem + "None.Label").classList.remove("active");
                $("#" + mandrelItem + "None.Label").popover({ content: "Invalid Bar Size for this Mandrel", trigger: "hover", placement: "bottom" }).popover('show');

                document.getElementById(mandrelItem + "Small").disabled = false;
                $("#" + mandrelItem + "Small.Label").popover("hide");

                document.getElementById(mandrelItem + "Medium").disabled = false;
                $("#" + mandrelItem + "Medium.Label").popover("hide");
                break;
            case "5":
                document.getElementById(mandrelItem + "None").disabled = true;
                document.getElementById(mandrelItem + "None").checked = false;
                document.getElementById(mandrelItem + "None.Label").classList.remove("active");
                $("#" + mandrelItem + "None.Label").popover({ content: "Invalid Bar Size for this Mandrel", trigger: "hover", placement: "bottom" }).popover('show');

                document.getElementById(mandrelItem + "Small").disabled = true;
                document.getElementById(mandrelItem + "Small").checked = false;
                document.getElementById(mandrelItem + "Small.Label").classList.remove("active");
                $("#" + mandrelItem + "Small.Label").popover({ content: "Invalid Bar Size for this Mandrel", trigger: "hover", placement: "bottom" }).popover('show');

                document.getElementById(mandrelItem + "Medium").disabled = false;
                $("#" + mandrelItem + "Medium.Label").popover("hide");
                break;
            case "6":
                document.getElementById(mandrelItem + "None").disabled = true;
                document.getElementById(mandrelItem + "None").checked = false;
                document.getElementById(mandrelItem + "None.Label").classList.remove("active");
                $("#" + mandrelItem + "None.Label").popover({ content: "Invalid Bar Size for this Mandrel", trigger: "hover", placement: "bottom" }).popover('show');

                document.getElementById(mandrelItem + "Small").disabled = true;
                document.getElementById(mandrelItem + "Small").checked = false;
                document.getElementById(mandrelItem + "Small.Label").classList.remove("active");
                $("#" + mandrelItem + "Small.Label").popover({ content: "Invalid Bar Size for this Mandrel", trigger: "hover", placement: "bottom" }).popover('show');

                document.getElementById(mandrelItem + "Medium").disabled = true;
                document.getElementById(mandrelItem + "Medium").checked = false;
                document.getElementById(mandrelItem + "Medium.Label").classList.remove("active");
                $("#" + mandrelItem + "Medium.Label").popover({ content: "Invalid Bar Size for this Mandrel", trigger: "hover", placement: "bottom" }).popover('show');
                break;
        }
    }
}

function getAllFormValues(form) {
    var fields = ["Qty", "BarSize", "Legs[0].Length"];//will add more fields as the shape increases
    var numbLegs = document.getElementById(form+'.LegCount').value;
    if (numbLegs >0) {
        for (var legIndex = 1; legIndex <= numbLegs; ++legIndex) {
            fields.push(`Legs[${(legIndex - 1)}].Degree`,`Legs[${(legIndex - 1)}].IsRight`, `Legs[${(legIndex - 1)}].Mandrel`,`Legs[${legIndex}].Length`);
        }
    }

    return fields;
}

function checkLegs(form) {
    var numbLegs = document.getElementById(form+'.LegCount').value;
    for (var legIndex = 0; legIndex <= numbLegs; ++legIndex) {
        var LegsLenght = document.getElementById(`${form}.leg[${legIndex}].lenght`).value;
        if (LegsLenght > 240) {
            alert("The leg #" + (legIndex + 1) + " can not be more than 240");//catching longer than vali
            return false;
        } else if (LegsLenght < 0) {
            alert("The leg #" + (legIndex + 1) + " can not be negative");//catching smaller than valid
            return false;
        } else if (legIndex < numbLegs && (document.getElementById(`${form}.leg[${legIndex}].degree`).value > 180 || 
            document.getElementById(`${form}.leg[${legIndex}].degree`).value <=0)) {
            alert("Invalid degrees in leg #" + (legIndex + 1));//catching smaller than valid value
            return false;
        }
    }
    return true; 
}

function checkingCutLenght(form) {
    fields = getAllFormValues(form);
    var numbLegs = document.getElementById(form+'.LegCount').value;

    var newShape = {
        ShapeID: "0",
        BarSize: document.forms[form]["BarSize"].value,
        LegCount: numbLegs,
        Qty: document.forms[form]["Qty"].value,
        NumCompleted: "",
        Legs: []
    };

    for (var legIndex = 0; legIndex <= numbLegs; ++legIndex) {
        var leg = { //filling up a leg with all necessary
            Length: document.forms[form][`Legs[${(legIndex)}].Length`].value,
            SortOrder: (legIndex + 1),
            Degree: document.forms[form][`Legs[${(legIndex)}].Degree`].value,
            Mandrel: { Name: document.forms[form][`Legs[${(legIndex)}].Mandrel`].value },
            IsRight: document.forms[form][`Legs[${(legIndex)}].IsRight`].value
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
                document.forms[form].submit();
            }
            
        }
    });
    return false;
}

function submitForm(form) {

    if (checkFormFieldsEmpty(form) && checkLegs(form)) {
        if (confirm('Are you sure you want to add this shape?'))
            checkingCutLenght(form);
    }
}

function deleteLeg(legAccordionID,form) { //deleting a leg and hidding previous properties
    document.getElementById(legAccordionID).remove();
    var numbLegs = document.getElementById(`${form}.LegCount`).value;
    
    document.getElementById(`${form}.leg[${numbLegs-1}].degreeLabel`).style.display = "none";
    document.getElementById(`${form}.leg[${numbLegs-1}].directionLabel`).style.display = "none";
    document.getElementById(`${form}.leg[${numbLegs-1}].direction`).style.display = "none";
    document.getElementById(`${form}.leg[${numbLegs - 1}].Mandrel`).style.display = "none";
    document.getElementById(`${form}.leg[${numbLegs-1}].delete`).style.display = "unset";
    document.getElementById(`${form}.LegCount`).value = numbLegs-1;
}