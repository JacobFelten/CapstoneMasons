var canvas = document.getElementById(canvasID);
//Always check for properties and methods, to make sure your code doesn't break in other browsers.
if (canvas.getContext) {

    //global variables for canvas
    //var thicc = 25;
    var scalar = 1;
    var min_x;
    var max_x;
    var min_y;
    var max_y;
    x_shift = 0;
    y_shift = 0;
    f_size = "12px Arial";
    var angle_shift = -9;
    var Too_Big = 0;
    //To be used to break out of while loop later
    var buffer = 25;
    var total_crude_length;


    //variables for shape
    var curr_unit_val = 0;
    var neg_pos = 1;
    var curr_x = 0;
    var curr_y = 0;
    var prev_x = 0;
    var prev_y = 0;

    var x0;
    var y0;
    var tempx0;
    var tempy0;

    //Variables for Shape creation
    var x1;
    var y1;
    var tempx1;
    var tempy1;

    var x2;
    var y2;
    var tempx2;
    var tempy2;

    var x3;
    var y3;
    var tempx3;
    var tempy3;

    var x4;
    var y4;
    var tempx4;
    var tempy4;

    var x5;
    var y5;
    var tempx5;
    var tempy5;

    var x6;
    var y6;
    var tempx6;
    var tempy6;

    var context = canvas.getContext('2d');

    scalar = 1;
    //reset scalar back to 1
    curr_x = (canvas.width) / 2;
    //sets inital x point
    curr_y = (canvas.height) / 2;
    //sets initial y point
    curr_unit_val = 0;
    //set unit circle value
    for (var j = 0; j < num_legs; j++) {
        if (j == 0) {

            x0 = curr_x;
            //save the new x coord
            y0 = curr_y;
            //save the new y coord
            min_x = x0;
            min_y = y0;
            max_x = x0;
            max_y = y0;

            x1 = x0 + leg1_length * scalar;
            //save the new x coord
            y1 = y0;
            //save the new y coord

            if (x1 < min_x) {
                min_x = x1;
            } else if (x1 > max_x) {
                max_x = x1;
            } else {//they are equal do nothing.
            }

            if (y1 < min_y) {
                min_y = y1;
            } else if (y1 > max_y) {
                max_y = y1;
            } else {//They are equal do nothing.
            }


        } else if (j == 1) {
            if (leg1_direction == "right") {
                neg_pos = 1;
            } else {
                neg_pos = -1;
            }

            //(x2,y2)
            x2 = (x1 + scalar * Math.cos(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg1_degree))) * leg2_length);
            y2 = (y1 + scalar * Math.sin(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg1_degree))) * leg2_length);

            if (x2 < min_x) {
                min_x = x2;
            } else if (x2 > max_x) {
                max_x = x2;
            } else {//they are equal do nothing.
            }

            if (y2 < min_y) {
                min_y = y2;
            } else if (y2 > max_y) {
                max_y = y2;
            } else {//They are equal do nothing.
            }

            curr_unit_val = curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg1_degree);

        } else if (j == 2) {
            if (leg2_direction == "right") {
                neg_pos = 1;
            } else {
                neg_pos = -1;
            }

            //(x3,y3)
            x3 = (x2 + scalar * Math.cos(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg2_degree))) * leg3_length);
            y3 = (y2 + scalar * Math.sin(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg2_degree))) * leg3_length);

            if (x3 < min_x) {
                min_x = x3;
            } else if (x3 > max_x) {
                max_x = x3;
            } else {//they are equal do nothing.
            }

            if (y3 < min_y) {
                min_y = y3;
            } else if (y3 > max_y) {
                max_y = y3;
            } else {//They are equal do nothing.
            }

            curr_unit_val = curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg2_degree);

        } else if (j == 3) {
            if (leg3_direction == "right") {
                neg_pos = 1;
            } else {
                neg_pos = -1;
            }

            //(x4,y4)
            x4 = (x3 + scalar * Math.cos(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg3_degree))) * leg4_length);
            y4 = (y3 + scalar * Math.sin(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg3_degree))) * leg4_length);

            if (x4 < min_x) {
                min_x = x4;
            } else if (x4 > max_x) {
                max_x = x4;
            } else {//they are equal do nothing.
            }

            if (y4 < min_y) {
                min_y = y4;
            } else if (y4 > max_y) {
                max_y = y4;
            } else {//They are equal do nothing.
            }

            curr_unit_val = curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg3_degree);

        } else if (j == 4) {
            if (leg4_direction == "right") {
                neg_pos = 1;
            } else {
                neg_pos = -1;
            }

            //(x5,y5)
            x5 = (x4 + scalar * Math.cos(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg4_degree))) * leg5_length);
            y5 = (y4 + scalar * Math.sin(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg4_degree))) * leg5_length);

            if (x5 < min_x) {
                min_x = x5;
            } else if (x5 > max_x) {
                max_x = x5;
            } else {//they are equal do nothing.
            }

            if (y5 < min_y) {
                min_y = y5;
            } else if (y5 > max_y) {
                max_y = y5;
            } else {//They are equal do nothing.
            }

            curr_unit_val = curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg4_degree);

        } else if (j == 5) {
            if (leg5_direction == "right") {
                neg_pos = 1;
            } else {
                neg_pos = -1;
            }

            //(x6,y6)
            x6 = (x5 + scalar * Math.cos(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg5_degree))) * leg6_length);
            y6 = (y5 + scalar * Math.sin(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg5_degree))) * leg6_length);

            if (x6 < min_x) {
                min_x = x6;
            } else if (x6 > max_x) {
                max_x = x6;
            } else {//they are equal do nothing.
            }

            if (y6 < min_y) {
                min_y = y6;
            } else if (y6 > max_y) {
                max_y = y6;
            } else {//They are equal do nothing.
            }

            curr_unit_val = curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg5_degree);

        }
    }

    //NOW THAT THE COORDS ARE SET AND MAX MIN ARE SET. CALCULATE X and Y Shift values
    x_shift = (canvas.width / 2) - ((max_x + min_x) / 2);
    y_shift = (canvas.height / 2) - ((max_y + min_y) / 2);

    //RESET CURR_UNIT_VAL
    curr_unit_val = 0;

    //set x0 and y0 to new position
    x0 = x0 + x_shift;
    y0 = y0 + y_shift;

    //Redraw the shape after x0 and y0 has been repositioned
    for (var j = 0; j < num_legs; j++) {
        if (j == 0) {

            min_x = x0;
            min_y = y0;
            max_x = x0;
            max_y = y0;

            //draw first line of shape
            context.beginPath();
            context.fillstyle = "red";

            //(x0,y0)
            context.moveTo(x0, y0);

            x1 = x0 + leg1_length * scalar;
            //save the new x coord
            y1 = y0;
            //save the new y coord

            if (x1 < min_x) {
                min_x = x1;
            } else if (x1 > max_x) {
                max_x = x1;
            } else {//they are equal do nothing.
            }

            if (y1 < min_y) {
                min_y = y1;
            } else if (y1 > max_y) {
                max_y = y1;
            } else {//They are equal do nothing.
            }

            //(x1,y1)
            context.lineTo(x1, y1);

            context.stroke();
            //draw the actual line

            //now draw text on top of the line itself

        } else if (j == 1) {
            if (leg1_direction == "right") {
                neg_pos = 1;
            } else {
                neg_pos = -1;
            }

            context.beginPath();
            context.moveTo(x1, y1);

            //(x2,y2)
            x2 = (x1 + scalar * Math.cos(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg1_degree))) * leg2_length);
            y2 = (y1 + scalar * Math.sin(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg1_degree))) * leg2_length);

            if (x2 < min_x) {
                min_x = x2;
            } else if (x2 > max_x) {
                max_x = x2;
            } else {//they are equal do nothing.
            }

            if (y2 < min_y) {
                min_y = y2;
            } else if (y2 > max_y) {
                max_y = y2;
            } else {//They are equal do nothing.
            }

            context.lineTo(x2, y2);

            curr_unit_val = curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg1_degree);

            context.stroke();
            //draw the actual line

            //draw dimensions on top of line
        } else if (j == 2) {
            if (leg2_direction == "right") {
                neg_pos = 1;
            } else {
                neg_pos = -1;
            }

            context.beginPath();
            context.moveTo(x2, y2);

            //(x3,y3)
            x3 = (x2 + scalar * Math.cos(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg2_degree))) * leg3_length);
            y3 = (y2 + scalar * Math.sin(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg2_degree))) * leg3_length);

            if (x3 < min_x) {
                min_x = x3;
            } else if (x3 > max_x) {
                max_x = x3;
            } else {//they are equal do nothing.
            }

            if (y3 < min_y) {
                min_y = y3;
            } else if (y3 > max_y) {
                max_y = y3;
            } else {//They are equal do nothing.
            }

            context.lineTo(x3, y3);

            curr_unit_val = curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg2_degree);

            context.stroke();
            //draw the actual line

            //draw dimensions on top of line
        } else if (j == 3) {
            if (leg3_direction == "right") {
                neg_pos = 1;
            } else {
                neg_pos = -1;
            }

            context.beginPath();
            context.moveTo(x3, y3);

            //(x4,y4)
            x4 = (x3 + scalar * Math.cos(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg3_degree))) * leg4_length);
            y4 = (y3 + scalar * Math.sin(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg3_degree))) * leg4_length);

            if (x4 < min_x) {
                min_x = x4;
            } else if (x4 > max_x) {
                max_x = x4;
            } else {//they are equal do nothing.
            }

            if (y4 < min_y) {
                min_y = y4;
            } else if (y4 > max_y) {
                max_y = y4;
            } else {//They are equal do nothing.
            }

            context.lineTo(x4, y4);

            curr_unit_val = curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg3_degree);

            context.stroke();
            //draw the actual line

            //draw dimensions on top of line
        } else if (j == 4) {
            if (leg4_direction == "right") {
                neg_pos = 1;
            } else {
                neg_pos = -1;
            }

            context.beginPath();
            context.moveTo(x4, y4);

            //(x5,y5)
            x5 = (x4 + scalar * Math.cos(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg4_degree))) * leg5_length);
            y5 = (y4 + scalar * Math.sin(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg4_degree))) * leg5_length);

            if (x5 < min_x) {
                min_x = x5;
            } else if (x5 > max_x) {
                max_x = x5;
            } else {//they are equal do nothing.
            }

            if (y5 < min_y) {
                min_y = y5;
            } else if (y5 > max_y) {
                max_y = y5;
            } else {//They are equal do nothing.
            }

            context.lineTo(x5, y5);

            curr_unit_val = curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg4_degree);

            context.stroke();
            //draw the actual line

            //draw dimensions on top of line
        } else if (j == 5) {
            if (leg5_direction == "right") {
                neg_pos = 1;
            } else {
                neg_pos = -1;
            }

            context.beginPath();
            context.moveTo(x5, y5);

            //(x6,y6)
            x6 = (x5 + scalar * Math.cos(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg5_degree))) * leg6_length);
            y6 = (y5 + scalar * Math.sin(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg5_degree))) * leg6_length);

            if (x6 < min_x) {
                min_x = x6;
            } else if (x6 > max_x) {
                max_x = x6;
            } else {//they are equal do nothing.
            }

            if (y6 < min_y) {
                min_y = y6;
            } else if (y6 > max_y) {
                max_y = y6;
            } else {//They are equal do nothing.
            }

            context.lineTo(x6, y6);

            curr_unit_val = curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg5_degree);

            context.stroke();
            //draw the actual line

            //draw dimensions on top of line

            //console.log("Min X is: " + min_x + " Max X is: " + max_x);
            //console.log("Min Y is: " + min_y + " Max Y is: " + max_y);

            console.log("Middle of tiny shape currently lies on (" + (max_x + min_x) / 2 + "," + (max_y + min_y) / 2 + ")");

        }
        //End of center-draw for loop
    }
    //End of Legs in a single shape loop

    //Before While LOOP set the temp coordinates up!
    tempx0 = x0;
    tempy0 = y0;

    //while Too_big != 1 loop
    while (Too_Big != 1) {
        //make scalar a little bigger
        scalar = scalar + .1;
        curr_unit_val = 0;

        //Recalculate the shape coordinates based on scalar
        for (var j = 0; j < num_legs; j++) {
            if (j == 0) {
                //reset the min max val
                min_x = tempx0;
                min_y = tempy0;
                max_x = tempx0;
                max_y = tempy0;

                tempx1 = tempx0 + leg1_length * scalar;
                //save the new x coord
                tempy1 = tempy0;
                //save the new y coord

                if (tempx1 < min_x) {
                    min_x = tempx1;
                } else if (tempx1 > max_x) {
                    max_x = tempx1;
                } else {//they are equal do nothing.
                }

                if (tempy1 < min_y) {
                    min_y = tempy1;
                } else if (tempy1 > max_y) {
                    max_y = tempy1;
                } else {//They are equal do nothing.
                }

            } else if (j == 1) {
                if (leg1_direction == "right") {
                    neg_pos = 1;
                } else {
                    neg_pos = -1;
                }

                //(x2,y2)
                tempx2 = (tempx1 + scalar * Math.cos(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg1_degree))) * leg2_length);
                tempy2 = (tempy1 + scalar * Math.sin(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg1_degree))) * leg2_length);

                if (tempx2 < min_x) {
                    min_x = tempx2;
                } else if (tempx2 > max_x) {
                    max_x = tempx2;
                } else {//they are equal do nothing.
                }

                if (tempy2 < min_y) {
                    min_y = tempy2;
                } else if (tempy2 > max_y) {
                    max_y = tempy2;
                } else {//They are equal do nothing.
                }

                curr_unit_val = curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg1_degree);

            } else if (j == 2) {
                if (leg2_direction == "right") {
                    neg_pos = 1;
                } else {
                    neg_pos = -1;
                }

                //(x3,y3)
                tempx3 = (tempx2 + scalar * Math.cos(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg2_degree))) * leg3_length);
                tempy3 = (tempy2 + scalar * Math.sin(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg2_degree))) * leg3_length);

                if (tempx3 < min_x) {
                    min_x = tempx3;
                } else if (tempx3 > max_x) {
                    max_x = tempx3;
                } else {//they are equal do nothing.
                }

                if (tempy3 < min_y) {
                    min_y = tempy3;
                } else if (tempy3 > max_y) {
                    max_y = tempy3;
                } else {//They are equal do nothing.
                }

                curr_unit_val = curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg2_degree);

            } else if (j == 3) {
                if (leg3_direction == "right") {
                    neg_pos = 1;
                } else {
                    neg_pos = -1;
                }

                //(x4,y4)
                tempx4 = (tempx3 + scalar * Math.cos(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg3_degree))) * leg4_length);
                tempy4 = (tempy3 + scalar * Math.sin(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg3_degree))) * leg4_length);

                if (tempx4 < min_x) {
                    min_x = tempx4;
                } else if (tempx4 > max_x) {
                    max_x = tempx4;
                } else {//they are equal do nothing.
                }

                if (tempy4 < min_y) {
                    min_y = tempy4;
                } else if (tempy4 > max_y) {
                    max_y = tempy4;
                } else {//They are equal do nothing.
                }

                curr_unit_val = curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg3_degree);

            } else if (j == 4) {
                if (leg4_direction == "right") {
                    neg_pos = 1;
                } else {
                    neg_pos = -1;
                }

                //(x5,y5)
                tempx5 = (tempx4 + scalar * Math.cos(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg4_degree))) * leg5_length);
                tempy5 = (tempy4 + scalar * Math.sin(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg4_degree))) * leg5_length);

                if (tempx5 < min_x) {
                    min_x = tempx5;
                } else if (tempx5 > max_x) {
                    max_x = tempx5;
                } else {//they are equal do nothing.
                }

                if (tempy5 < min_y) {
                    min_y = tempy5;
                } else if (tempy5 > max_y) {
                    max_y = tempy5;
                } else {//They are equal do nothing.
                }

                curr_unit_val = curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg4_degree);

            } else if (j == 5) {
                if (leg5_direction == "right") {
                    neg_pos = 1;
                } else {
                    neg_pos = -1;
                }

                //(x6,y6)
                tempx6 = (tempx5 + scalar * Math.cos(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg5_degree))) * leg6_length);
                tempy6 = (tempy5 + scalar * Math.sin(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg5_degree))) * leg6_length);

                if (tempx6 < min_x) {
                    min_x = tempx6;
                } else if (tempx6 > max_x) {
                    max_x = tempx6;
                } else {//they are equal do nothing.
                }

                if (tempy6 < min_y) {
                    min_y = tempy6;
                } else if (tempy6 > max_y) {
                    max_y = tempy6;
                } else {//They are equal do nothing.
                }

                curr_unit_val = curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg5_degree);

            }
        }

        //shift shape over to center based on scaled version
        x_shift = (canvas.width / 2) - ((max_x + min_x) / 2);
        y_shift = (canvas.height / 2) - ((max_y + min_y) / 2);

        tempx0 = tempx0 + x_shift;
        tempy0 = tempy0 + y_shift;

        curr_unit_val = 0
        //calculate new positions based on scalar and check new mins and maxes
        for (var j = 0; j < num_legs; j++) {

            if (j == 0) {
                //reset the min max val
                min_x = tempx0;
                min_y = tempy0;
                max_x = tempx0;
                max_y = tempy0;

                tempx1 = tempx0 + leg1_length * scalar;
                //save the new x coord
                tempy1 = tempy0;
                //save the new y coord

                if (tempx1 < min_x) {
                    min_x = tempx1;
                } else if (tempx1 > max_x) {
                    max_x = tempx1;
                } else {//they are equal do nothing.
                }

                if (tempy1 < min_y) {
                    min_y = tempy1;
                } else if (tempy1 > max_y) {
                    max_y = tempy1;
                } else {//They are equal do nothing.
                }

            } else if (j == 1) {
                if (leg1_direction == "right") {
                    neg_pos = 1;
                } else {
                    neg_pos = -1;
                }

                //(x2,y2)
                tempx2 = (tempx1 + scalar * Math.cos(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg1_degree))) * leg2_length);
                tempy2 = (tempy1 + scalar * Math.sin(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg1_degree))) * leg2_length);

                if (tempx2 < min_x) {
                    min_x = tempx2;
                } else if (tempx2 > max_x) {
                    max_x = tempx2;
                } else {//they are equal do nothing.
                }

                if (tempy2 < min_y) {
                    min_y = tempy2;
                } else if (tempy2 > max_y) {
                    max_y = tempy2;
                } else {//They are equal do nothing.
                }

                curr_unit_val = curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg1_degree);

            } else if (j == 2) {
                if (leg2_direction == "right") {
                    neg_pos = 1;
                } else {
                    neg_pos = -1;
                }

                //(x3,y3)
                tempx3 = (tempx2 + scalar * Math.cos(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg2_degree))) * leg3_length);
                tempy3 = (tempy2 + scalar * Math.sin(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg2_degree))) * leg3_length);

                if (tempx3 < min_x) {
                    min_x = tempx3;
                } else if (tempx3 > max_x) {
                    max_x = tempx3;
                } else {//they are equal do nothing.
                }

                if (tempy3 < min_y) {
                    min_y = tempy3;
                } else if (tempy3 > max_y) {
                    max_y = tempy3;
                } else {//They are equal do nothing.
                }

                curr_unit_val = curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg2_degree);

            } else if (j == 3) {
                if (leg3_direction == "right") {
                    neg_pos = 1;
                } else {
                    neg_pos = -1;
                }

                //(x4,y4)
                tempx4 = (tempx3 + scalar * Math.cos(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg3_degree))) * leg4_length);
                tempy4 = (tempy3 + scalar * Math.sin(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg3_degree))) * leg4_length);

                if (tempx4 < min_x) {
                    min_x = tempx4;
                } else if (tempx4 > max_x) {
                    max_x = tempx4;
                } else {//they are equal do nothing.
                }

                if (tempy4 < min_y) {
                    min_y = tempy4;
                } else if (tempy4 > max_y) {
                    max_y = tempy4;
                } else {//They are equal do nothing.
                }

                curr_unit_val = curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg3_degree);

            } else if (j == 4) {
                if (leg4_direction == "right") {
                    neg_pos = 1;
                } else {
                    neg_pos = -1;
                }

                //(x5,y5)
                tempx5 = (tempx4 + scalar * Math.cos(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg4_degree))) * leg5_length);
                tempy5 = (tempy4 + scalar * Math.sin(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg4_degree))) * leg5_length);

                if (tempx5 < min_x) {
                    min_x = tempx5;
                } else if (tempx5 > max_x) {
                    max_x = tempx5;
                } else {//they are equal do nothing.
                }

                if (tempy5 < min_y) {
                    min_y = tempy5;
                } else if (tempy5 > max_y) {
                    max_y = tempy5;
                } else {//They are equal do nothing.
                }

                curr_unit_val = curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg4_degree);

            } else if (j == 5) {
                if (leg5_direction == "right") {
                    neg_pos = 1;
                } else {
                    neg_pos = -1;
                }

                //(x6,y6)
                tempx6 = (tempx5 + scalar * Math.cos(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg5_degree))) * leg6_length);
                tempy6 = (tempy5 + scalar * Math.sin(((Math.PI) / 180) * (curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg5_degree))) * leg6_length);

                if (tempx6 < min_x) {
                    min_x = tempx6;
                } else if (tempx6 > max_x) {
                    max_x = tempx6;
                } else {//they are equal do nothing.
                }

                if (tempy6 < min_y) {
                    min_y = tempy6;
                } else if (tempy6 > max_y) {
                    max_y = tempy6;
                } else {//They are equal do nothing.
                }

                curr_unit_val = curr_unit_val + (neg_pos) * (180) - (neg_pos) * (leg5_degree);

            }
        }

        //if min and max are within bounds then save the coords permanently and repeat
        if (min_x > (0 + buffer) && max_x < (canvas.width - buffer) && min_y > (0 + buffer) && max_y < (canvas.height - buffer)) {
            x0 = tempx0;
            y0 = tempy0;
            x1 = tempx1;
            y1 = tempy1;
            x2 = tempx2;
            y2 = tempy2;
            x3 = tempx3;
            y3 = tempy3;
            x4 = tempx4;
            y4 = tempy4;
            x5 = tempx5;
            y5 = tempy5;
            x6 = tempx6;
            y6 = tempy6;
        }
        //else  when shapes is equal to or greater than the bounds of the canvas then don't save the values of coords and set Too_Big equal to 1
        else {
            Too_Big = 1;
        }
    }

    //Draw the final shape after it has been scaled up!

    context.lineJoin = "round";

    if (total_crude_length > 0 && total_crude_length <= 50) {
        context.lineWidth = 2 * scalar;
    } else if (total_crude_length > 50 && total_crude_length <= 100) {
        context.lineWidth = 3 * scalar;
    } else if (total_crude_length > 100 && total_crude_length <= 150) {
        context.lineWidth = 4 * scalar;
    } else if (total_crude_length > 150 && total_crude_length <= 200) {
        context.lineWidth = 5 * scalar;
    } else if (total_crude_length > 200 && total_crude_length <= 240) {
        context.lineWidth = 6 * scalar;
    } else {
        context.lineWidth = 1 * scalar;
    }

    context.beginPath();
    context.moveTo(x0, y0);
    context.lineTo(x1, y1);
    context.lineTo(x2, y2);
    context.lineTo(x3, y3);
    context.lineTo(x4, y4);
    context.lineTo(x5, y5);
    context.lineTo(x6, y6);
    context.stroke();

    //End of if(canvas.getcontext) block
}
