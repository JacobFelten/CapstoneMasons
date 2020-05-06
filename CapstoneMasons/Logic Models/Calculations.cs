using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneMasons.Models;
using CapstoneMasons.Infrastructure;

namespace CapstoneMasons.Logic_Models
{
    public static class Calculations
    {


        //NOT USED ANYMORE BECAUSE I STOPPED BEING STUBBORN AND USED STUPID LISTS
        //public double[] crude_legs = new double[leg_num]; //sets the array to be as big as the number of legs declared for the shape and stores the customer-provided lengths of the legs

        //public double[] true_legs = new double[leg_num];  //sets the array to be as big as the number of legs declared for the shape and stores the true lengths of all legs (straight segments of shape)

        //public double[,] angles = new double[leg_num - 1, 2]; //Creates an array big enough for all angle and their respective Mandrel radii. [Angle, Mandrel]


        //method to convert all customer given leg values to true leg values (dimensions of only the truely straight areas)
        //Must have legs array and angles array completed before executing or else won't work.
        public static void Convert_Legs_To_True_Legs(Shape s, in List<Leg> crude_legs, in List<Leg> true_legs)
        {
            decimal Thickness = ((decimal)s.BarSize) / 8; //converts the given rebar type to the actual thickness of the rebar in decimal form

            for (int leg = 0; leg < crude_legs.Count; leg++)
            {
                true_legs.Add(new Leg());
                //Hits the if statement if it is on the first leg of the shape
                if (leg == 0)
                {
                    if (crude_legs.Count == 1)
                    {
                        true_legs[leg].Length = crude_legs[leg].Length;
                    }
                    else
                    {
                        true_legs[leg].Length = crude_legs[leg].Length - ((decimal)Math.Tan(crude_legs[leg].Degree / 2) * crude_legs[leg].Mandrel.Radius * Thickness);  //converts the customer leg value to the true leg value and populates the true_legs array
                    }
                }

                //Hits else if statement if it is on the last leg of the shape
                else if (leg == crude_legs.Count - 1)
                {
                    true_legs[leg].Length = crude_legs[leg].Length - ((decimal)Math.Tan(crude_legs[leg - 1].Degree / 2) * crude_legs[leg - 1].Mandrel.Radius * Thickness);  //converts the customer leg value to the true leg value and populates the true_legs array
                }

                //Hits else statement if it is on anything other than the first or last leg of the shape
                else
                {
                    true_legs[leg].Length = crude_legs[leg].Length - ((decimal)Math.Tan(crude_legs[leg - 1].Degree / 2) * crude_legs[leg - 1].Mandrel.Radius * Thickness) - ((decimal)Math.Tan(crude_legs[leg].Degree / 2) * crude_legs[leg].Mandrel.Radius * Thickness);  //converts the customer leg value to the true leg value and populates the true_legs array
                }
            }

            return; //When task of converting legs to true legs has completed
        }

        public static decimal Total_Shape_Length(Shape s) //so far returns the total length of the shape based on the true leg values and angle/mandrel values
        {
            decimal K_Factor = .446M; //Global unit for K_Factor......Could be different, but for most materials it is .446 inches
            decimal Thickness = s.BarSize / 8; //converts the given rebar type to the actual thickness of the rebar in decimal form
            decimal Pi = 3.141592653589793238M;


            //List of crude dimensions of the legs given by the customer
            List<Leg> crude_legs = s.Legs;
       
            List<Leg> true_legs = new List<Leg>();

            



        //Converts all innaccurate leg values to true leg values before calculating the total_shape_length
        Convert_Legs_To_True_Legs(s, in crude_legs, in true_legs);

            decimal total_shape_length = 0;

            //First for loop adds up all the leg lengths in the legs array and adds them to total_shape_length
            
            
            for (int leg = 0; leg < true_legs.Count; leg++)  // I believe it should be leg < true_legs.length not leg <= true_legs.length
            {
                total_shape_length += true_legs[leg].Length;
            }
            
            //Added -1 to the rude_legs.Count. Might fuck it up

            //Second for loop adds up all the Bend Allowance values in the shape
            for (int angle = 0; angle < crude_legs.Count - 1; angle++) // I believe it should be angle < angles.length not angle <= angles.length
            {
                total_shape_length += (((Pi / 180) * crude_legs[angle].Mandrel.Radius) + ((Pi / 180) * K_Factor * Thickness)) * (crude_legs[angle].Degree);
            }

            total_shape_length = Math.Round(total_shape_length * KnownObjects.RoundToNth, MidpointRounding.ToEven) / KnownObjects.RoundToNth;

            return total_shape_length; //returns the summation of all the parts of the shape
        }

        //Method returns the amount of this shape able to be made out of a 20' bar rounded to the nearest whole shape


        //revise this as more info is received for the cost per each rebar type
        
    }
}
