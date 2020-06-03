//legs is the JS object that holds each leg each with their own Length, Degree, and IsRight values
function DrawCanvas(canvasID, legs)
{
var canvas = document.getElementById(canvasID);
//Always check for properties and methods, to make sure your code doesn't break in other browsers.
if (canvas.getContext) {

    //global variables for canvas
    var BarSize = legs[0].Size; //sets the barsize from the legs being passed in
    var scalar = 1;
    var min_x;
    var max_x;
    var min_y;
    var max_y;
    x_shift = 0;
    y_shift = 0;
    var Too_Big = 0;
    var thicc = 10;
    var max_thicc = 20;

    //To be used to break out of while loop later
    var buffer = 25;
    var total_crude_length;

    //variables for shape
    var curr_unit_val = 0;
    var neg_pos = 1;
    var curr_x = 0;
    var curr_y = 0;


    //variables needed for hooks
    OSSBarcx1 = {};
    OSSBarcy1 = {};
    OSSBarcx2 = {};
    OSSBarcy2 = {};
    Arcx1 = {};
    Arcy1 = {};
    Arcx2 = {};
    Arcy2 = {};
    ArcMidx = {};
    ArcMidy = {};

    HookIndexes = {};  //the indexes of legs that are hooks
    var HOOKSCALAR = 10;

    //Variables for Shape creation


    var context = canvas.getContext('2d');

    startx = 0;  //the initial x value of the shape
    starty = 0;  //the initial y value of the shape

    //initialize x and y arrays
    xcoords = {};
    ycoords = {};


    //variables for drawing mandrel radii onto shapes
    //initialize OSSB point arrays
    ossbX1 = {};
    ossbY1 = {};
    ossbX2 = {};
    ossbY2 = {};
    OSSB_length = {};

    //initialize the Apex point arrays
    apexX = {};
    apexY = {};




    //Keep track of the unit circle positions for potential Leg Labeling
    unit_val_list = {};


    scalar = 1;
    //reset scalar back to 1
    curr_x = (canvas.width) / 2;
    //sets inital x point
    curr_y = (canvas.height) / 2;
    //sets initial y point
    curr_unit_val = 0;
    //set unit circle value

    var addedPoints = 0;

    for (var j = 0; j < legs.length; j++) {
        //if first leg of shape
        if (j == 0) {

            startx = curr_x;
            starty = curr_y;

            xcoords[0] = startx;
            ycoords[0] = starty;

            //save the new x coord
            //save the new y coord
            min_x = curr_x;
            min_y = curr_y;
            max_x = curr_x;
            max_y = curr_y;

            xcoords[1] = xcoords[j] + legs[j].Length;
            //save the new x coord
            ycoords[1] = ycoords[j]
            //save the new y coord

            if (xcoords[j + 1] < min_x) {
                min_x = xcoords[j + 1];
            } else if (xcoords[j + 1] > max_x) {
                max_x = xcoords[j + 1];
            } else {//they are equal do nothing.
            }

            if (ycoords[j + 1] < min_y) {
                min_y = ycoords[j + 1];
            } else if (ycoords[j + 1] > max_y) {
                max_y = ycoords[j + 1];
            } else {//They are equal do nothing.
            }

            //add the starting unit circle value to the list
            unit_val_list[j] = curr_unit_val;

        //else other legs of shape
        } else {
            if (legs[j - 1].IsRight == true) {
                neg_pos = 1;
            } else {
                neg_pos = -1;
            }

            //if degree of the last leg is 180 then find flipped point position, else do nothing different
            if (legs[j - 1].Degree == 180) {

                var tempCoordx, tempCoordy, tempReverseAngle;
                tempCoordx = xcoords[j + addedPoints] + Math.cos(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * 90)) * legs[j - 1].Mandrel * 2 * 10;
                tempCoordy = ycoords[j + addedPoints] + Math.sin(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * 90)) * legs[j - 1].Mandrel * 2 * 10;

                xcoords[j + 1 + addedPoints] = tempCoordx;
                ycoords[j + 1 + addedPoints] = tempCoordy;

                if (xcoords[j + 1 + addedPoints] < min_x) {
                    min_x = xcoords[j + 1 + addedPoints];
                } else if (xcoords[j + 1 + addedPoints] > max_x) {
                    max_x = xcoords[j + 1 + addedPoints];
                } else {//they are equal do nothing.
                }

                if (ycoords[j + 1 + addedPoints] < min_y) {
                    min_y = ycoords[j + 1 + addedPoints];
                } else if (ycoords[j + 1 + addedPoints] > max_y) {
                    max_y = ycoords[j + 1 + addedPoints];
                } else {//They are equal do nothing.
                }


                //INSIDE OF HERE SAVE THE CURRENT LEG INDEX THAT IS THE HOOK AND PUT IT INTO THE HOOK INDEX LIST using Object.keys(HookIndexes).length
                //SAVE J as the leg being counted as the Hook and add it to the end of the indexes list
                HookIndexes[Object.keys(HookIndexes).length] = j + addedPoints;

                //increment j by 1
                addedPoints = addedPoints + 1;

                //possibly useless actually
                //tempReverseAngle = curr_unit_val - (neg_pos)(180);

                //draws the line from the temp point in the opposite direction than the last line
                xcoords[j + 1 + addedPoints] = tempCoordx + Math.cos(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * 360)) * legs[j].Length;
                ycoords[j + 1 + addedPoints] = tempCoordy + Math.sin(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * 360)) * legs[j].Length;

                curr_unit_val = curr_unit_val + (neg_pos) * (180) - (neg_pos) * 360;

                if (xcoords[j + 1 + addedPoints] < min_x) {
                    min_x = xcoords[j + 1 + addedPoints];
                } else if (xcoords[j + 1 + addedPoints] > max_x) {
                    max_x = xcoords[j + 1 + addedPoints];
                } else {//they are equal do nothing.
                }

                if (ycoords[j + 1 + addedPoints] < min_y) {
                    min_y = ycoords[j + 1 + addedPoints];
                } else if (ycoords[j + 1 + addedPoints] > max_y) {
                    max_y = ycoords[j + 1 + addedPoints];
                } else {//They are equal do nothing.
                }
            }
            else {

                //(xj+1,yj+1)
                xcoords[j + 1 + addedPoints] = xcoords[j + addedPoints] + Math.cos(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (legs[j - 1].Degree))) * legs[j].Length;
                ycoords[j + 1 + addedPoints] = ycoords[j + addedPoints] + Math.sin(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (legs[j - 1].Degree))) * legs[j].Length;

                curr_unit_val = curr_unit_val + (neg_pos) * (180) - (neg_pos) * (legs[j - 1].Degree);

                if (xcoords[j + 1 + addedPoints] < min_x) {
                    min_x = xcoords[j + 1 + addedPoints];
                } else if (xcoords[j + 1 + addedPoints] > max_x) {
                    max_x = xcoords[j + 1 + addedPoints];
                } else {//they are equal do nothing.
                }

                if (ycoords[j + 1 + addedPoints] < min_y) {
                    min_y = ycoords[j + 1 + addedPoints];
                } else if (ycoords[j + 1 + addedPoints] > max_y) {
                    max_y = ycoords[j + 1 + addedPoints];
                } else {//They are equal do nothing.
                }
            }


            //make sure that curr_unit_val never gets bigger than 180 or smaller than -180
            while (curr_unit_val > 180 || curr_unit_val < -180)
            {
                if (curr_unit_val > 180) {
                    curr_unit_val = curr_unit_val - 360;
                }
                else
                {
                    curr_unit_val = curr_unit_val + 360;
                }
            }

            //save curr_unit_val to the list
            unit_val_list[j] = curr_unit_val;
        }
    }

    //NOW THAT THE COORDS ARE SET AND MAX MIN ARE SET. CALCULATE X and Y Shift values
    x_shift = (canvas.width / 2) - ((max_x + min_x) / 2);
    y_shift = (canvas.height / 2) - ((max_y + min_y) / 2);

    //RESET CURR_UNIT_VAL
    curr_unit_val = 0;

    //set x0 and y0 to new position
    startx = startx + x_shift;
    starty = starty + y_shift;

    //Clear xcoords and ycoords
    xcoords = [];
    ycoords = [];


    //Redraw the shape after x0 and y0 has been repositioned
    addedPoints = 0;

    for (var j = 0; j < legs.length; j++) {
        if (j == 0) {

            xcoords[0] = startx;
            ycoords[0] = starty;

            min_x = curr_x;
            min_y = curr_y;
            max_x = curr_x;
            max_y = curr_y;


            //draw first line of shape
                //context.beginPath();

            //(x0,y0)
                //context.moveTo(xcoords[0], ycoords[0]);


            xcoords[1] = xcoords[j] + legs[j].Length;
            //save the new x coord
            ycoords[1] = ycoords[j];
            //save the new y coord

            if (xcoords[j + 1] < min_x) {
                min_x = xcoords[j + 1];
            } else if (xcoords[j + 1] > max_x) {
                max_x = xcoords[j + 1];
            } else {//they are equal do nothing.
            }

            if (ycoords[j + 1] < min_y) {
                min_y = ycoords[j + 1];
            } else if (ycoords[j + 1] > max_y) {
                max_y = ycoords[j + 1];
            } else {//They are equal do nothing.
            }


            //(x1,y1)
                //context.lineTo(xcoords[1], ycoords[1]);

                //context.stroke();
            //draw the actual line


        } else {
            if (legs[j - 1].IsRight == true) {
                neg_pos = 1;
            } else {
                neg_pos = -1;
            }

            //if degree of last leg is 180
            if (legs[j - 1].Degree == 180) {

                if (j < legs.length) {
                    var tempCoordx, tempCoordy, tempReverseAngle;
                    tempCoordx = xcoords[j + addedPoints] + Math.cos(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * 90)) * legs[j - 1].Mandrel * 2 * HOOKSCALAR;
                    tempCoordy = ycoords[j + addedPoints] + Math.sin(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * 90)) * legs[j - 1].Mandrel * 2 * HOOKSCALAR;

                    xcoords[j + 1 + addedPoints] = tempCoordx;
                    ycoords[j + 1 + addedPoints] = tempCoordy;

                    if (xcoords[j + 1 + addedPoints] < min_x) {
                        min_x = xcoords[j + 1 + addedPoints];
                    } else if (xcoords[j + 1 + addedPoints] > max_x) {
                        max_x = xcoords[j + 1 + addedPoints];
                    } else {//they are equal do nothing.
                    }

                    if (ycoords[j + 1 + addedPoints] < min_y) {
                        min_y = ycoords[j + 1 + addedPoints];
                    } else if (ycoords[j + 1 + addedPoints] > max_y) {
                        max_y = ycoords[j + 1 + addedPoints];
                    } else {//They are equal do nothing.
                    }

                                //Draw first part of the hook
                                //context.lineTo(xcoords[j + 1 + addedPoints], ycoords[j + 1 + addedPoints]);

                    //increment j by 1
                    addedPoints = addedPoints + 1;

                    //draws the line from the temp point in the opposite direction than the last line
                    xcoords[j + 1 + addedPoints] = tempCoordx + Math.cos(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * 360)) * legs[j].Length;
                    ycoords[j + 1 + addedPoints] = tempCoordy + Math.sin(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * 360)) * legs[j].Length;

                                //Draw second part of the hook
                                //context.lineTo(xcoords[j + 1 + addedPoints], ycoords[j + 1 + addedPoints]);

                    curr_unit_val = curr_unit_val + (neg_pos) * (180) - (neg_pos) * 360;

                    if (xcoords[j + 1 + addedPoints] < min_x) {
                        min_x = xcoords[j + 1 + addedPoints];
                    } else if (xcoords[j + 1 + addedPoints] > max_x) {
                        max_x = xcoords[j + 1 + addedPoints];
                    } else {//they are equal do nothing.
                    }

                    if (ycoords[j + 1 + addedPoints] < min_y) {
                        min_y = ycoords[j + 1 + addedPoints];
                    } else if (ycoords[j + 1 + addedPoints] > max_y) {
                        max_y = ycoords[j + 1 + addedPoints];
                    } else {//They are equal do nothing.
                    }
                }
            }
            else {

                if (j < legs.length) {
                    //(xj+1,yj+1)
                    xcoords[j + 1 + addedPoints] = xcoords[j + addedPoints] + Math.cos(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (legs[j - 1].Degree))) * legs[j].Length;
                    ycoords[j + 1 + addedPoints] = ycoords[j + addedPoints] + Math.sin(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (legs[j - 1].Degree))) * legs[j].Length;

                                //Draw the line
                                //context.lineTo(xcoords[j + 1 + addedPoints], ycoords[j + 1 + addedPoints]);

                    curr_unit_val = curr_unit_val + (neg_pos) * (180) - (neg_pos) * (legs[j - 1].Degree);

                    if (xcoords[j + 1 + addedPoints] < min_x) {
                        min_x = xcoords[j + 1 + addedPoints];
                    } else if (xcoords[j + 1 + addedPoints] > max_x) {
                        max_x = xcoords[j + 1 + addedPoints];
                    } else {//they are equal do nothing.
                    }

                    if (ycoords[j + 1 + addedPoints] < min_y) {
                        min_y = ycoords[j + 1 + addedPoints];
                    } else if (ycoords[j + 1 + addedPoints] > max_y) {
                        max_y = ycoords[j + 1 + addedPoints];
                    } else {//They are equal do nothing.
                    }
                }
            }

                    //Draw the lines from lineTo()
                    //context.stroke();
        }
    }//End of Legs in a single shape loop

    //console.log("Current Shape is on " + canvasID);
    //console.log("Middle of tiny shape currently lies on (" + (max_x + min_x) / 2 + "," + (max_y + min_y) / 2 + ")");







    //Before While LOOP set the temp coordinates up!
    temp_xcoords = {};
    temp_ycoords = {};
    temp_xcoords[0] = xcoords[0];
    temp_ycoords[0] = ycoords[0];


    //TO HELP WITH THE HOOKS BEING NOTICABLE
    
    addedPoints = 0;
    HOOKSCALAR = 2;
    var MANDREL_SCALAR = 0;

    //while Too_big != 1 loop
    while (Too_Big != 1) {
        //make scalar a little bigger
        addedPoints = 0;
        scalar = scalar + .1;
        curr_unit_val = 0;

        if (scalar <= 2) {
            //HOOKSCALAR huge
            HOOKSCALAR = 12;
        }
        else if (scalar > 2 && scalar <= 4) {
            //HOOKSCALAR bigger
            HOOKSCALAR = 8;
        }
        else if (scalar > 4 && scalar <= 6) {
            //HOOKSCALAR big
            HOOKSCALAR = 6;
        }
        else if (scalar > 6 && scalar <= 8) {
            //HOOKSCALAR small
            HOOKSCALAR = 4;
        }
        else if (scalar > 8 && scalar <= 10) {
            //HOOKSCALAR smallest
            HOOKSCALAR = 3;
        }
        else if (scalar > 10 && scalar <= 30) {
            //HOOKSCALAR smaller than microorganisms
            HOOKSCALAR = 2;
        }
        else if (scalar > 30) {
            HOOKSCALAR = 1;
        }

        //Recalculate the shape coordinates based on scalar
        for (var j = 0; j < legs.length; j++) {
            //if first leg
            if (j == 0) {
                //reset the min max val
                min_x = temp_xcoords[0];
                min_y = temp_ycoords[0];
                max_x = temp_xcoords[0];
                max_y = temp_ycoords[0];

                temp_xcoords[1] = temp_xcoords[0] + legs[0].Length * scalar;
                //save the new x coord
                temp_ycoords[1] = temp_ycoords[0];
                //save the new y coord

                if (temp_xcoords[1] < min_x) {
                    min_x = temp_xcoords[1];
                } else if (temp_xcoords[1] > max_x) {
                    max_x = temp_xcoords[1];
                } else {//they are equal do nothing.
                }

                if (temp_ycoords[1] < min_y) {
                    min_y = temp_ycoords[1];
                } else if (temp_ycoords[1] > max_y) {
                    max_y = temp_ycoords[1];
                } else {//They are equal do nothing.
                }

            //if other legs
            } else {
                if (legs[j - 1].IsRight == true) {
                    neg_pos = 1;
                } else {
                    neg_pos = -1;
                }

                //if degree of the last leg is 180 then find flipped point position, else do nothing different
                if (legs[j - 1].Degree == 180) {

                    //if none mandrel
                    if (legs[j - 1].Mandrel == .5) {
                        //don't change the hook distance
                        MANDREL_SCALAR = 1.1;
                    }
                    //if small mandrel
                    else if (legs[j - 1].Mandrel == 1) {
                        //shorten it by a little more than what it would be
                        MANDREL_SCALAR = .6;
                    }
                    //if medium
                    else if (legs[j - 1].Mandrel == 1.5) {
                        //shorten it by even more
                        MANDREL_SCALAR = .5
                    }
                    //if large mandrel
                    else if (legs[j - 1].Mandrel == 2.25) {
                        //shorten it by a lot more
                        MANDREL_SCALAR = .4;
                    }

                    var tempCoordx, tempCoordy, tempReverseAngle;
                    tempCoordx = temp_xcoords[j + addedPoints] + scalar * Math.cos(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * 90)) * legs[j - 1].Mandrel * 2 * HOOKSCALAR * MANDREL_SCALAR; //HOOKSCALAR;
                    tempCoordy = temp_ycoords[j + addedPoints] + scalar * Math.sin(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * 90)) * legs[j - 1].Mandrel * 2 * HOOKSCALAR * MANDREL_SCALAR; //HOOKSCALAR;

                    temp_xcoords[j + 1 + addedPoints] = tempCoordx;
                    temp_ycoords[j + 1 + addedPoints] = tempCoordy;

                    if (temp_xcoords[j + 1 + addedPoints] < min_x) {
                        min_x = temp_xcoords[j + 1 + addedPoints];
                    } else if (temp_xcoords[j + 1 + addedPoints] > max_x) {
                        max_x = temp_xcoords[j + 1 + addedPoints];
                    } else {//they are equal do nothing.
                    }

                    if (temp_ycoords[j + 1 + addedPoints] < min_y) {
                        min_y = temp_ycoords[j + 1 + addedPoints];
                    } else if (temp_ycoords[j + 1 + addedPoints] > max_y) {
                        max_y = temp_ycoords[j + 1 + addedPoints];
                    } else {//They are equal do nothing.
                    }

                    //increment j by 1
                    addedPoints = addedPoints + 1;


                    //draws the line from the temp point in the opposite direction than the last line
                    temp_xcoords[j + 1 + addedPoints] = tempCoordx + scalar * Math.cos(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * 360)) * legs[j].Length;
                    temp_ycoords[j + 1 + addedPoints] = tempCoordy + scalar * Math.sin(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * 360)) * legs[j].Length;

                    curr_unit_val = curr_unit_val + (neg_pos) * (180) - (neg_pos) * 360;

                    if (temp_xcoords[j + 1 + addedPoints] < min_x) {
                        min_x = temp_xcoords[j + 1 + addedPoints];
                    } else if (temp_xcoords[j + 1 + addedPoints] > max_x) {
                        max_x = temp_xcoords[j + 1 + addedPoints];
                    } else {//they are equal do nothing.
                    }

                    if (temp_ycoords[j + 1 + addedPoints] < min_y) {
                        min_y = temp_ycoords[j + 1 + addedPoints];
                    } else if (temp_ycoords[j + 1 + addedPoints] > max_y) {
                        max_y = temp_ycoords[j + 1 + addedPoints];
                    } else {//They are equal do nothing.
                    }
                }

                else {
                    //(xj+1,yj+1)
                    temp_xcoords[j + 1 + addedPoints] = (temp_xcoords[j + addedPoints] + scalar * Math.cos(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (legs[j - 1].Degree))) * legs[j].Length);
                    temp_ycoords[j + 1 + addedPoints] = (temp_ycoords[j + addedPoints] + scalar * Math.sin(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (legs[j - 1].Degree))) * legs[j].Length);


                    curr_unit_val = curr_unit_val + (neg_pos) * (180) - (neg_pos) * (legs[j - 1].Degree);

                    if (temp_xcoords[j + 1 + addedPoints] < min_x) {
                        min_x = temp_xcoords[j + 1 + addedPoints];
                    } else if (temp_xcoords[j + 1 + addedPoints] > max_x) {
                        max_x = temp_xcoords[j + 1 + addedPoints];
                    } else {//they are equal do nothing.
                    }

                    if (temp_ycoords[j + 1 + addedPoints] < min_y) {
                        min_y = temp_ycoords[j + 1 + addedPoints];
                    } else if (temp_ycoords[j + 1 + addedPoints] > max_y) {
                        max_y = temp_ycoords[j + 1 + addedPoints];
                    } else {//They are equal do nothing.
                    }
                }

            }
        }

        //shift shape over to center based on scaled version
        x_shift = (canvas.width / 2) - ((max_x + min_x) / 2);
        y_shift = (canvas.height / 2) - ((max_y + min_y) / 2);

        var x_start = temp_xcoords[0] + x_shift;
        var y_start = temp_ycoords[0] + y_shift;

        temp_xcoords = [];
        temp_ycoords = [];

        temp_xcoords[0] = x_start;
        temp_ycoords[0] = y_start;


        curr_unit_val = 0
        addedPoints = 0;

        //calculate new positions based on scalar and check new mins and maxes
        for (var j = 0; j < legs.length; j++) {

            //if first leg
            if (j == 0) {
                //reset the min max val
                min_x = temp_xcoords[0];
                min_y = temp_ycoords[0];
                max_x = temp_xcoords[0];
                max_y = temp_ycoords[0];

                temp_xcoords[1] = temp_xcoords[0] + legs[0].Length * scalar;
                //save the new x coord
                temp_ycoords[1] = temp_ycoords[0];
                //save the new y coord

                if (temp_xcoords[1] < min_x) {
                    min_x = temp_xcoords[1];
                } else if (temp_xcoords[1] > max_x) {
                    max_x = temp_xcoords[1];
                } else {//they are equal do nothing.
                }

                if (temp_ycoords[1] < min_y) {
                    min_y = temp_ycoords[1];
                } else if (temp_ycoords[1] > max_y) {
                    max_y = temp_ycoords[1];
                } else {//They are equal do nothing.
                }

            //if other legs
            } else {
                if (legs[j - 1].IsRight == true) {
                    neg_pos = 1;
                } else {
                    neg_pos = -1;
                }

                //if degree of the last leg is 180 then find flipped point position, else do nothing different
                if (legs[j - 1].Degree == 180) {

                    //editing the distance of the hooks depending on the mandrel radius
                    if (legs[j - 1].Mandrel == .5) {
                        //don't change the hook distance
                        MANDREL_SCALAR = 1.1;
                    }
                    //if small mandrel
                    else if (legs[j - 1].Mandrel == 1) {
                        //shorten it by a little more than what it would be
                        MANDREL_SCALAR = .6;
                    }
                    //if medium
                    else if (legs[j - 1].Mandrel == 1.5) {
                        //shorten it by even more
                        MANDREL_SCALAR = .5
                    }
                    //if large mandrel
                    else if (legs[j - 1].Mandrel == 2.25) {
                        //shorten it by a lot more
                        MANDREL_SCALAR = .4;
                    }

                    var tempCoordx, tempCoordy, tempReverseAngle;
                    tempCoordx = temp_xcoords[j + addedPoints] + scalar * Math.cos(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * 90)) * legs[j - 1].Mandrel * 2 * HOOKSCALAR * MANDREL_SCALAR;
                    tempCoordy = temp_ycoords[j + addedPoints] + scalar * Math.sin(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * 90)) * legs[j - 1].Mandrel * 2 * HOOKSCALAR * MANDREL_SCALAR;

                    temp_xcoords[j + 1 + addedPoints] = tempCoordx;
                    temp_ycoords[j + 1 + addedPoints] = tempCoordy;

                    if (temp_xcoords[j + 1 + addedPoints] < min_x) {
                        min_x = temp_xcoords[j + 1 + addedPoints];
                    } else if (temp_xcoords[j + 1 + addedPoints] > max_x) {
                        max_x = temp_xcoords[j + 1 + addedPoints];
                    } else {//they are equal do nothing.
                    }

                    if (temp_ycoords[j + 1 + addedPoints] < min_y) {
                        min_y = temp_ycoords[j + 1 + addedPoints];
                    } else if (temp_ycoords[j + 1 + addedPoints] > max_y) {
                        max_y = temp_ycoords[j + 1 + addedPoints];
                    } else {//They are equal do nothing.
                    }

                    //increment j by 1
                    addedPoints = addedPoints + 1;

                    //draws the line from the temp point in the opposite direction than the last line
                    temp_xcoords[j + 1 + addedPoints] = tempCoordx + scalar * Math.cos(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * 360)) * legs[j].Length;
                    temp_ycoords[j + 1 + addedPoints] = tempCoordy + scalar * Math.sin(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * 360)) * legs[j].Length;

                    curr_unit_val = curr_unit_val + (neg_pos) * (180) - (neg_pos) * 360;

                    if (temp_xcoords[j + 1 + addedPoints] < min_x) {
                        min_x = temp_xcoords[j + 1 + addedPoints];
                    } else if (temp_xcoords[j + 1 + addedPoints] > max_x) {
                        max_x = temp_xcoords[j + 1 + addedPoints];
                    } else {//they are equal do nothing.
                    }

                    if (temp_ycoords[j + 1 + addedPoints] < min_y) {
                        min_y = temp_ycoords[j + 1 + addedPoints];
                    } else if (temp_ycoords[j + 1 + addedPoints] > max_y) {
                        max_y = temp_ycoords[j + 1 + addedPoints];
                    } else {//They are equal do nothing.
                    }
                }

                else {
                    //(xj+1,yj+1)
                    temp_xcoords[j + 1 + addedPoints] = (temp_xcoords[j + addedPoints] + scalar * Math.cos(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (legs[j - 1].Degree))) * legs[j].Length);
                    temp_ycoords[j + 1 + addedPoints] = (temp_ycoords[j + addedPoints] + scalar * Math.sin(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (legs[j - 1].Degree))) * legs[j].Length);


                    curr_unit_val = curr_unit_val + (neg_pos) * (180) - (neg_pos) * (legs[j - 1].Degree);

                    if (temp_xcoords[j + 1 + addedPoints] < min_x) {
                        min_x = temp_xcoords[j + 1 + addedPoints];
                    } else if (temp_xcoords[j + 1 + addedPoints] > max_x) {
                        max_x = temp_xcoords[j + 1 + addedPoints];
                    } else {//they are equal do nothing.
                    }

                    if (temp_ycoords[j + 1 + addedPoints] < min_y) {
                        min_y = temp_ycoords[j + 1 + addedPoints];
                    } else if (temp_ycoords[j + 1 + addedPoints] > max_y) {
                        max_y = temp_ycoords[j + 1 + addedPoints];
                    } else {//They are equal do nothing.
                    }

                }

            }
        }

        //if min and max are within bounds then save the coords permanently and repeat
        if (min_x > (0 + buffer) && max_x < (canvas.width - buffer) && min_y > (0 + buffer) && max_y < (canvas.height - buffer)) {
            xcoords = temp_xcoords;
            ycoords = temp_ycoords;
        }
        //else  when shapes is equal to or greater than the bounds of the canvas then don't save the values of coords and set Too_Big equal to 1
        else {
            Too_Big = 1;
        }
    }

                    //Now that shape has been scaled up find out where the APEX and OSSB points are
                    curr_unit_val = 0

                    //Hibernating Code for Mandrel implementation

                    ////Save the Apex point values
                    //for (var coord = 0; coord < Object.keys(xcoords).length - 2; coord++) {
                    //    apexX[coord] = xcoords[coord + 1];
                    //    apexY[coord] = ycoords[coord + 1];
                    //}

                    //for (var j = 0; j < legs.length; j++) {

                    //    //as long as its not the last leg of the shape, save these temp variables
                    //    if (j != legs.length - 1)
                    //    { 
                    //        var Thickness_mm = (BarSize / 8) * 25.4;
                    //        var Radius_mm = legs[j].Mandrel * 25.4;
                    //        var comp_angle = 180 - legs[j].Degree;
                    //        var radian_angle = comp_angle * Math.PI / 180;
                    //        var OSSB = Math.tan(radian_angle / 2) * (Thickness_mm + Radius_mm);
                    //        OSSB = OSSB / 25.4;         //convert OSSB back into inches from millimeters
                    //        OSSB_length[j] = OSSB;
            
                    //    }
                    //    //if leg is the first leg
                    //    if (j == 0)
                    //    {
                    //        ossbX1[j] = xcoords[0] + legs[0].Length * scalar - OSSB_length[j] * scalar;
                    //        //save the OSSB point where the arcTo first begins
                    //        ossbY1[j] = ycoords[0];
                    //        //save the OSSB point where the arcTo first begins
                    //    }
                    //    //if the leg is the last leg
                    //    else if (j == legs.length - 1)
                    //    {
                    //        if (legs[j - 1].IsRight == true) {
                    //            neg_pos = 1;
                    //        }
                    //        else {
                    //            neg_pos = -1;
                    //        }

                    //        //save the previous OSSB point
                    //        ossbX2[j - 1] = xcoords[j] + scalar * Math.cos(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (legs[j - 1].Degree))) * OSSB_length[j - 1];
                    //        ossbY2[j - 1] = ycoords[j] + scalar * Math.sin(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (legs[j - 1].Degree))) * OSSB_length[j - 1];

                    //        curr_unit_val = curr_unit_val + (neg_pos) * (180) - (neg_pos) * (legs[j - 1].Degree);
                    //    }
                    //    //if leg is any leg between the first and last leg
                    //    else
                    //    {
                    //        if (legs[j - 1].IsRight == true)
                    //        {
                    //            neg_pos = 1;
                    //        }
                    //        else
                    //        {
                    //            neg_pos = -1;
                    //        }

                    //        //save the previous OSSB point
                    //        ossbX2[j - 1] = xcoords[j] + scalar * Math.cos(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (legs[j - 1].Degree))) * OSSB_length[j - 1];
                    //        ossbY2[j - 1] = ycoords[j] + scalar * Math.sin(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (legs[j - 1].Degree))) * OSSB_length[j - 1];

                    //        //save the next OSSB point
                    //        ossbX1[j] = xcoords[j] + scalar * Math.cos(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (legs[j - 1].Degree))) * legs[j].Length - scalar * Math.cos(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (legs[j - 1].Degree))) * OSSB_length[j];
                    //        ossbY1[j] = ycoords[j] + scalar * Math.sin(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (legs[j - 1].Degree))) * legs[j].Length - scalar * Math.sin(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (legs[j - 1].Degree))) * OSSB_length[j];

                    //        curr_unit_val = curr_unit_val + (neg_pos) * (180) - (neg_pos) * (legs[j - 1].Degree);

                    //    }
                    //}







    //Draw the final shape after it has been scaled up!

    context.lineJoin = "round";

    //Find shape's total length
    var total_crude_length = 0;
    for (var k = 0; k < legs.length; k++)
    {
        total_crude_length = total_crude_length + legs[k].Length;
    }

    //sets the line width according to the size of the Rebar
    if (BarSize == 3) {
        thicc = 15;
        //thicc = 10;
    }
    else if (BarSize == 4)
    {
        thicc = 17;
        //thicc = 12;
    }
    else if (BarSize == 5)
    {
        thicc = 19;
        //thicc = 14;
    }
    else if (BarSize == 6)
    {
        thicc = 21;
        //thicc = 16;
    }

    context.lineWidth = thicc;

    //context.lineWidth = 1;
    //get size of xcoord object to know how many coords are in it
    var num_coords = Object.keys(xcoords).length;

    context.beginPath();
    context.moveTo(xcoords[0], ycoords[0]);


    //Draw the big boi shape
    for (var coord = 1; coord < num_coords; coord++)
    {
        context.lineTo(xcoords[coord], ycoords[coord]);
        context.stroke();
    }


                            //MANDREL_CODE
                            //loop through the OSSB and APEX points and draw the shape with mandrel
                            //for (var coord = 0; coord < num_coords - 1; coord++)
                            //{
                            //    context.lineTo(ossbX1[coord], ossbY1[coord]);
                            //    context.stroke();
                            //    context.arcTo(apexX[coord], apexY[coord], ossbX2[coord], ossbY2[coord], legs[coord].Mandrel * scalar);
                            //    context.stroke();
                            //}
                            ////draw the remaining portion of the last leg of the shape
                            //context.lineTo(xcoords[num_coords - 1], ycoords[num_coords - 1]);


                            //context.stroke();


    //Text control variables
    f_size = "12px Arial";
    context.font = f_size;
    context.fillStyle = "white";
    text_x_shift = 0;
    text_y_shift = 0;
    text_2digit_x_shift = 0;
    text_2digit_y_shift = 0;

                                //Getting the points of where text is on a shape
                                //var label_posX = {};
                                //var label_posY = {};
                                //for (var leg = 0; leg < num_coords - 1; leg++)
                                //{
                                //    //if first leg
                                //    if (leg == 0)
                                //    {
                                //        label_posX[leg] = (xcoords[leg] + ossbX1[leg]) / 2;
                                //        label_posY[leg] = (ycoords[leg] + ossbY1[leg]) / 2;
                                //    }
                                //    //if last leg
                                //    else if (leg == num_coords - 2)
                                //    {
                                //        label_posX[leg] = (ossbX2[leg - 1] + xcoords[num_coords - 1]) / 2;
                                //        label_posY[leg] = (ossbY2[leg - 1] + ycoords[num_coords - 1]) / 2;
                                //    }
                                //    //if any other leg between first and last
                                //    else
                                //    {
                                //        label_posX[leg] = (ossbX2[leg - 1] + ossbX1[leg]) / 2;
                                //        label_posY[leg] = (ossbY2[leg - 1] + ossbY1[leg]) / 2;
                                //    }
                                //}



    //Count of how many hooks were skipped over
    var hooks_skipped = 0;
    var hook_found = 0;


    //Drawing the text onto the shape
    context.beginPath();
    context.moveTo(xcoords[0], ycoords[0]);
    for (var coord = 1; coord < num_coords; coord++)
    {
        //reset hook_found
        hook_found = 0;



        //introduce a text shift value dependant on the curr_unit_val at that spot
        //may get harder if introducing a number with two digits or more
        text_x_shift = 0;
        text_y_shift = 0;
        text_2digit_x_shift = 0;
        text_2digit_y_shift = 0;

        //if unit val is at 0 or 180 or -180 THEN shift straight down
        if (unit_val_list[coord - 1 - hooks_skipped] == 0 || unit_val_list[coord - 1 - hooks_skipped] == 180 || unit_val_list[coord - 1 - hooks_skipped] == -180) {
            if (coord - hooks_skipped > 9) {
                text_2digit_x_shift = -5;
            }
            text_y_shift = -4;
        }

        //if unit val is between 0 and -30 or between -180 and -150 THEN diagonal down left
        else if (0 > unit_val_list[coord - 1 - hooks_skipped] && unit_val_list[coord - 1 - hooks_skipped] > -30 || -150 > unit_val_list[coord - 1 - hooks_skipped] && unit_val_list[coord - 1 - hooks_skipped] > -180) {
            if (coord - hooks_skipped > 9) {
                text_2digit_x_shift = -3;
            }
            text_x_shift = -3;
            text_y_shift = -3;
        }

        //if unit val is at -30 or 150 THEN diagonal down left MORE LEFT THAN DOWN
        else if (unit_val_list[coord - 1 - hooks_skipped] == -30 || unit_val_list[coord - 1 - hooks_skipped] == 150) {
            if (coord - hooks_skipped > 9) {
                text_2digit_x_shift = -3;
            }
            text_x_shift = -3;
            text_y_shift = -3;
        }

        //if unit between -30 and -45 or between 135 and 150 THEN diagonal down left
        else if (-30 > unit_val_list[coord - 1 - hooks_skipped] && unit_val_list[coord - 1 - hooks_skipped] > -45 || 150 > unit_val_list[coord - 1 - hooks_skipped] && unit_val_list[coord - 1 - hooks_skipped] > 135) {
            if (coord - hooks_skipped > 9) {
                text_2digit_x_shift = -3;
            }
            text_x_shift = -3;
            text_y_shift = -3;
        }

        //if unit val is at -45 or 135 THEN do nothing
        else if (unit_val_list[coord - 1 - hooks_skipped] == -45 || unit_val_list[coord - 1 - hooks_skipped] == 135) {
            if (coord - hooks_skipped > 9) {
                text_2digit_x_shift = -3;
            }
        }

        //if unit between -45 and -60 or between 135 and 120 THEN diagonal down left
        else if (-45 > unit_val_list[coord - 1 - hooks_skipped] && unit_val_list[coord - 1 - hooks_skipped] > -60 || 135 > unit_val_list[coord - 1 - hooks_skipped] && unit_val_list[coord - 1 - hooks_skipped] > 120) {
            if (coord - hooks_skipped > 9) {
                text_2digit_x_shift = -3;
            }
            text_x_shift = -3;
            text_y_shift = -3;
        }

        //if unit val is at -60 or 120 THEN diagonal down left
        else if (unit_val_list[coord - 1 - hooks_skipped] == -60 || unit_val_list[coord - 1 - hooks_skipped] == 120) {
            if (coord - hooks_skipped > 9) {
                text_2digit_x_shift = -3;
            }
            text_x_shift = -4;
            text_y_shift = -4;
        }

        //if unit is between -60 and -90 or 120 and 90 THEN left
        else if (-60 > unit_val_list[coord - 1 - hooks_skipped] && unit_val_list[coord - 1 - hooks_skipped] > -90 || 120 > unit_val_list[coord - 1 - hooks_skipped] && unit_val_list[coord - 1 - hooks_skipped] > 90) {
            if (coord - hooks_skipped > 9) {
                text_2digit_x_shift = -3;
            }
            text_x_shift = -3;
        }

        //if unit val is at -90 or 90 THEN move to the left
        else if (unit_val_list[coord - 1 - hooks_skipped] == -90 || unit_val_list[coord - 1 - hooks_skipped] == 90) {
            if (coord - hooks_skipped > 9) {
                text_2digit_x_shift = -3;
            }
            text_x_shift = -4;
        }

        //if unit is between -90 and -120 or 90 and 60 THEN diagonal down left MORE LEFT THAN DOWN
        else if (-90 > unit_val_list[coord - 1 - hooks_skipped] && unit_val_list[coord - 1 - hooks_skipped] > -120 || 90 > unit_val_list[coord - 1 - hooks_skipped] && unit_val_list[coord - 1 - hooks_skipped] > 60) {
            if (coord - hooks_skipped > 9) {
                text_2digit_x_shift = -3;
            }
            text_x_shift = -4;
            text_y_shift = -2;
        }

        //if unit val is at -120 or 60 THEN diagonal down left MORE LEFT THAN DOWN
        else if (unit_val_list[coord - 1 - hooks_skipped] == -120 || unit_val_list[coord - 1 - hooks_skipped] == 60) {
            if (coord - hooks_skipped > 9) {
                text_2digit_x_shift = -4;
            }
            text_x_shift = -4;
            text_y_shift = -2;
        }

        //if unit is between -120 and -135 or 60 and 45 THEN diagonal down left
        else if (-120 > unit_val_list[coord - 1 - hooks_skipped] && unit_val_list[coord - 1 - hooks_skipped] > -135 || 60 > unit_val_list[coord - 1 - hooks_skipped] && unit_val_list[coord - 1 - hooks_skipped] > 45) {
            if (coord - hooks_skipped > 9) {
                text_2digit_x_shift = -3;
            }
            text_x_shift = -3;
            text_y_shift = -3;
        }

        //if unit val is at -135 or 45 THEN diagonal down left
        else if (unit_val_list[coord - 1 - hooks_skipped] == -135 || unit_val_list[coord - 1 - hooks_skipped] == 45) {
            if (coord - hooks_skipped > 9) {
                text_2digit_x_shift = -4;
            }
            text_x_shift = -4;
            text_y_shift = -4;
        }

        //if unit val is between -135 and -150 or 45 and 30 THEN diagonal down left MORE DOWN THAN LEFT
        else if (-135 > unit_val_list[coord - 1 - hooks_skipped] && unit_val_list[coord - 1 - hooks_skipped] > -150 || 45 > unit_val_list[coord - 1 - hooks_skipped] && unit_val_list[coord - 1 - hooks_skipped] > 30) {
            if (coord - hooks_skipped > 9) {
                text_2digit_x_shift = -3;
            }
            text_x_shift = -2;
            text_y_shift = -4;
        }

        //if unit val is at -150 or 30 THEN diagonal down left MORE DOWN THAN LEFT
        else if (unit_val_list[coord - 1 - hooks_skipped] == -150 || unit_val_list[coord - 1 - hooks_skipped] == 30) {
            if (coord - hooks_skipped > 9) {
                text_2digit_x_shift = -4;
                text_2digit_y_shift = -1;
            }
            text_x_shift = -3;
            text_y_shift = -4;
        }

        //if unit val is between 150 and 180 or -30 and 0 THEN 
        else if (180 > unit_val_list[coord - 1 - hooks_skipped] && unit_val_list[coord - 1 - hooks_skipped] > 150 || 30 > unit_val_list[coord - 1 - hooks_skipped] && unit_val_list[coord - 1 - hooks_skipped] > 0) {
            if (coord - hooks_skipped > 9) {
                text_2digit_x_shift = -3;
            }
            text_x_shift = -3;
            text_y_shift = -5;
        }
        else {
            //Do nothing!!
        }

        //context.fillText((coord).toString(), text_2digit_x_shift + text_x_shift + label_posX[coord - 1], (-1) * text_2digit_y_shift + (-1) * text_y_shift + label_posY[coord - 1]);


        for (var index = 0; index < Object.keys(HookIndexes).length; index++) {

            if (HookIndexes[index] == coord - 1) {
                //Leg is found to be a Hook and won't be allowed to label
                hook_found = 1;
            }
            else {
                //do nothing
            }
        }

        if (hook_found == 1) {
            hooks_skipped = hooks_skipped + 1;
        }
        else {
            //places labels on the segments themselves
            context.fillText((coord - hooks_skipped).toString(), text_2digit_x_shift + text_x_shift + (xcoords[coord - 1] + xcoords[coord]) / 2, (-1) * text_2digit_y_shift + (-1) * text_y_shift + (ycoords[coord - 1] + ycoords[coord]) / 2);

        }

    }
    context.stroke();

    //console.log("Middle of Big Boi shape on currently lies on (" + (max_x + min_x) / 2 + "," + (max_y + min_y) / 2 + ")");
    //console.log("Shape Total Crude Length: " + total_crude_length);
    //console.log("-------------------------------------------------------------------------------")

    //End of if(canvas.getcontext) block
    }

}
