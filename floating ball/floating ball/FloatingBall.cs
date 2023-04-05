﻿using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;

///@author Saad Turky / igsaturk
///@version 27.11.2018
/// <summary>
/// floating ballpeli
/// </summary>
public class FloatingBall : PhysicsGame
{
    PhysicsObject ball2;

    private Image[] BgImages = LoadImages("wall", "wall1", "cup");

    private Image star = LoadImage("tahti");

    private Image ball = LoadImage("ball");

    private SoundEffect bomb = LoadSoundEffect("explosion1");

    private SoundEffect eating = LoadSoundEffect("eat");

    private SoundEffect victory = LoadSoundEffect("victory");

    const int stars = 2;

    const int stars1 = 1;

    private int collectedStars = 0;

    private int map = 1;

    IntMeter pointsCounter;

    /// <summary>
    /// To count the number of the stars.
    /// </summary>
    public void PointsCounter()
    {
        pointsCounter = new IntMeter(0);
        Label pisteNaytto = new Label("collectedStars");
        pisteNaytto.X = Screen.Left + 900;
        pisteNaytto.Y = Screen.Top - 180;
        pisteNaytto.TextColor = Color.Black;
        pisteNaytto.Color = Color.White;
        pisteNaytto.Title = "Points";
        pointsCounter.Value += 1;
        pointsCounter.Value = 0;
        ///pisteLaskuri.AddOverTime(3, 5);
        /// pisteNaytto.IntFormatString = "Pisteitä: {0:D1}";
        pisteNaytto.BindTo(pointsCounter);
        Add(pisteNaytto);
    }
    

    /// <summary>
    /// To control with ball's vector 
    /// </summary>
    /// <param name="suunta"></param>
    private void HittingVector(Vector suunta)
    {
        ball2.Velocity = suunta;
    }


    /// <summary>
    /// To create the ball (it is a physicsobject).
    /// </summary>
    /// <param name="game"></param>
    /// <param name="x"> the position of the ball on the coordinate x</param>
    /// <param name="y"> the position of the ball on the coordinate y</param>
    public void DrawBall(Game game, double x, double y)
    {
        ball2 = new PhysicsObject(2 * 25, 2 * 25, Shape.Circle);
        ball2.Position = new Vector(-450, -350);
        ball2.Color = Color.Red;
        ball2.Tag = "enemy";
        ball2.Image = ball;
        game.Add(ball2);
        ///AddCollisionHandler(ball2, "vihu", PelaajaTormasi);
      
    }


    /// <summary>
    /// creating the obstacles (it is a physicsobject).
    /// </summary>
    /// <param name="game"></param>
    /// <param name="x">the position of the ball on the coordinate x</param>
    /// <param name="y">the position of the ball on the coordinate y</param>
    /// <param name="w"> the width of the obstacle</param>
    /// <param name="h"> the height of the obstacle</param>
    /// <param name="color">color</param>
    public void DrawSquare(Game game, double x, double y, double w, double h, Color color)
    {
        PhysicsObject square = new PhysicsObject(w, h, Shape.Rectangle);
        square.X = x;
        square.Y = y;
        square.Color = color;
        square.MakeStatic();
        square.Image = LoadImage("obs");
        AddCollisionHandler(square, "enemy", CrushPlayer);
        game.Add(square);
    }


    /// <summary>
    /// the object that the ball must destroy it 
    /// </summary>
    /// <param name="game"></param>
    /// <param name="x">the position of the ball on the coordinate x</param>
    /// <param name="y">the position of the ball on the coordinate y</param>
    /// <param name="w">the width of the star</param>
    /// <param name="h">the height of the star</param>
    public void Stars(Game game, double x, double y, double w, double h)
    {
        PhysicsObject Shape1 = new PhysicsObject(w, h, Shape.Star);
        Shape1.X = x;
        Shape1.Y = y;
        Shape1.Color = Color.Red;
        /// Shape1.Tag = "Shape1";  
        Shape1.Image = star;
        AddCollisionHandler(Shape1, "enemy", DestroyStar);
        game.Add(Shape1);
    }


    /// <summary>
    /// to destroy the star
    /// </summary>
    /// <param name="Shape1">star</param>
    /// <param name="ball2">ball</param>
    public void DestroyStar(PhysicsObject Shape1, PhysicsObject ball2)
    {
        MessageDisplay.Add("Hyvä!");
        Shape1.Destroy();
        eating.Play();

        collectedStars++;
        pointsCounter.Value += 1;

        if (map == 1)
        {
            if (collectedStars >= stars)
            {
                collectedStars = 0;
                map++;
                SecondMap();
            }
        }

        if (map == 2)
        {
            if (collectedStars >= stars1)
            {
                map++;
                ThirdMap();
            }
        }
    }


    public override void Begin()
    {
        if (map == 1)
        {
            FirstMap();
        }

        if (map == 2)
        {
            SecondMap();
        }

        Buttons();
        MessageDisplay.Add("Taso " + map + "!");
        PointsCounter();
    }


   /// <summary>
   /// the explosion of the player
   /// </summary>
   /// <param name="player">ball</param>
   /// <param name="enemy">obstacle</param>
    public void CrushPlayer(PhysicsObject player, PhysicsObject enemy)
    {
        Explosion rajahdys = new Explosion(enemy.Width * 100);
        rajahdys.Position = enemy.Position;
        rajahdys.UseShockWave = false;
        this.Add(rajahdys);
        bomb.Play();
        Remove(enemy);
        MessageDisplay.Add("Yritä Uudelleen!");
        Timer.SingleShot(2.0, () => { ClearAll(); Begin();});
        collectedStars=0;
    }


    /// <summary>
    /// To control with bottons
    /// </summary>   
    public void Buttons()
    {
        Keyboard.Listen(Key.Up, ButtonState.Pressed, HittingVector, "move the ball to the up", new Vector(0, 100));
        Keyboard.Listen(Key.Down, ButtonState.Pressed, HittingVector, "move the ball to the down", new Vector(0, -100));
        Keyboard.Listen(Key.Left, ButtonState.Pressed, HittingVector, "move the ball to the left", new Vector(-100, 0));
        Keyboard.Listen(Key.Right, ButtonState.Pressed, HittingVector, "move the ball to the right", new Vector(100, 0));

        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");

    }


    /// <summary>
    /// The First Level
    /// </summary>
    public void FirstMap()
    {
        Level.Size = Screen.Size;
        // Level.Width = Screen.Width
        // Level.Height = Screen.Height;
        Camera.ZoomToLevel();
        Level.CreateBorders();
        Level.Background.Image = BgImages[0];
        MessageDisplay.Position = new Vector(20, 150);


        /// calling the Subroutine of the ball
        DrawBall(this, 0, 0);


        ///calling a subroutine of the obstacle (it is an object) to become six obstacles in the lower part
        int n = 6; //represent the number of the obstacles
        double h = 250;
        double w = 90;
        double xcoordinate = -290;
        double ycoordinate = -340;
        double oldboxsize = 0;

        for (int i = 0; i < n; i++)
        {
            oldboxsize = h;
            DrawSquare(this, xcoordinate, ycoordinate, w, h, Color.RosePink);
            xcoordinate = xcoordinate + 100 + 50;
            h = h * 0.5 + oldboxsize * 0.8;
        }


         ///calling a subroutine of the obstacle (it is an object) to become seven obstacles in the upper part
         int y = 7; //represent the number of the obstacles
         h = 150;
         w = 90;
         xcoordinate = -400;
         ycoordinate = 300;
        for (int i = 0; i < y; i++)
        {
            DrawSquare(this, xcoordinate, ycoordinate, w, h, Color.Green);
            xcoordinate = xcoordinate + 100 + 50;
        }


        /// calling the subroutine of the Stars for the lower part to become five stars
        ///int s = 6;
         h = 50;
         w = 50;
         xcoordinate = -215;
         ycoordinate = -350;
        for (int i = 1; i < n; i++)
        {
            Stars(this, xcoordinate, ycoordinate, w, h);
            xcoordinate = xcoordinate + 75 + 75;
        }


        /// calling the subroutine of the Stars for the upper part to become six stars
         int r = 7;   //represent the number of the stars
         xcoordinate = -325;
         ycoordinate = 350;
        for (int i = 1; i < r; i++)
        {
            Stars(this, xcoordinate, ycoordinate, w, h);
            xcoordinate = xcoordinate + 75 + 75;
        }
    }


    /// <summary>
    /// The Second Level
    /// </summary>
    public void SecondMap()
    {
        ClearAll();
        PointsCounter();
        MessageDisplay.Add("Taso 2!");
        Camera.ZoomToLevel();
        Level.CreateBorders();
        Level.Background.Image = BgImages[1];
        ///Level.Background.CreateGradient(Color.Magenta, Color.Yellow);


        /// calling the Subroutine of the ball
        DrawBall(this, 0, 0);


        ///calling a subroutine of the obstacle (it is an object) to become six obstacles
        int s = 6; //represent the number of the obstacles
        double h = 50;
        double w = 90;
        double xcoordinate = -320;
        double ycoordinate = -350;
        ///double oldboxsize = 0;

        for (int i = 0; i < s; i++)
        {
            ///oldboxsize = h;
            DrawSquare(this, xcoordinate, ycoordinate, w, h, Color.Blue);
            xcoordinate = xcoordinate + 100 + 50;
            /// h = h * 0.5 + oldboxsize * 0.8;
        }


        ///calling a subroutine of the obstacle (it is an object) to become six obstacles
        ///int a = 6;
        xcoordinate = -420;
        ycoordinate = -230;
        ///double oldboxsize = 0;

        for (int i = 0; i < s; i++)
        {
            ///oldboxsize = h;
            DrawSquare(this, xcoordinate, ycoordinate, w, h, Color.Red);
            xcoordinate = xcoordinate + 100 + 50;
            /// h = h * 0.5 + oldboxsize * 0.8;
        }


        ///calling a subroutine of the obstacle (it is an object) to become six obstacles
        ///int aa = 6;
        xcoordinate = -320;
        ycoordinate = -110;
        ///double oldboxsize = 0;

        for (int i = 0; i < s; i++)
        {
            ///oldboxsize = h;
            DrawSquare(this, xcoordinate, ycoordinate, w, h, Color.Pink);
            xcoordinate = xcoordinate + 100 + 50;
            /// h = h * 0.5 + oldboxsize * 0.8;
        }


        ///calling a subroutine of the obstacle (it is an object) to become six obstacles
        ///int d = 6;
        xcoordinate = -420;
        ycoordinate = 0;
        ///double oldboxsize = 0;

        for (int i = 0; i < s; i++)
        {
            ///oldboxsize = h;
            DrawSquare(this, xcoordinate, ycoordinate, w, h, Color.Orange);
            xcoordinate = xcoordinate + 100 + 50;
            /// h = h * 0.5 + oldboxsize * 0.8;
        }


        ///calling a subroutine of the obstacle (it is an object) to become six obstacles
        ///int t = 6;
        xcoordinate = -320;
        ycoordinate = 120;
        ///double oldboxsize = 0;

        for (int i = 0; i < s; i++)
        {
            ///oldboxsize = h;
            DrawSquare(this, xcoordinate, ycoordinate, w, h, Color.Purple);
            xcoordinate = xcoordinate + 100 + 50;
            /// h = h * 0.5 + oldboxsize * 0.8;
        }


        ///calling a subroutine of the obstacle (it is an object) to become six obstacles
        ///int u = 6;
        xcoordinate = -420;
        ycoordinate = 240;
        ///double oldboxsize = 0;

        for (int i = 0; i < s; i++)
        {
            ///oldboxsize = h;
            DrawSquare(this, xcoordinate, ycoordinate, w, h, Color.Magenta);
            xcoordinate = xcoordinate + 100 + 50;
            /// h = h * 0.5 + oldboxsize * 0.8;
        }


        ///calling a subroutine of the obstacle (it is an object) to become six obstacles
        ///int r = 6;
        xcoordinate = -320;
        ycoordinate = 350;
        ///double oldboxsize = 0;

        for (int i = 0; i < s; i++)
        {
            ///oldboxsize = h;
            DrawSquare(this, xcoordinate, ycoordinate, w, h, Color.Olive);
            xcoordinate = xcoordinate + 100 + 50;
            /// h = h * 0.5 + oldboxsize * 0.8;
        }


        /// calling the subroutine of the Stars to become five stars
        ///int k = 6;
         h = 30;
         w = 30;
         xcoordinate = -250;
         ycoordinate = -360;
        for (int i = 1; i < s; i++)
        {
            Stars(this, xcoordinate, ycoordinate, w, h);
            xcoordinate = xcoordinate + 75 + 75;
        }


        /// calling the subroutine of the Stars to become five stars
        ///int y = 6;

        xcoordinate = -350;
        ycoordinate = -230;
        for (int i = 1; i < s; i++)
        {
            Stars(this, xcoordinate, ycoordinate, w, h);
            xcoordinate = xcoordinate + 75 + 75;
        }


        /// calling the subroutine of the Stars to become five stars
        ///int g = 6;
        xcoordinate = -250;
        ycoordinate = -105;
        for (int i = 1; i < s; i++)
        {
            Stars(this, xcoordinate, ycoordinate, w, h);
            xcoordinate = xcoordinate + 75 + 75;
        }


        /// calling the subroutine of the Stars to become five stars
        ///int l = 6;
        xcoordinate = -350;
        ycoordinate = 0;
        for (int i = 1; i < s; i++)
        {
            Stars(this, xcoordinate, ycoordinate, w, h);
            xcoordinate = xcoordinate + 75 + 75;
        }


        /// calling the subroutine of the Stars to become five stars
        ///int m = 6;
        xcoordinate = -250;
        ycoordinate = 120;
        for (int i = 1; i < s; i++)
        {
            Stars(this, xcoordinate, ycoordinate, w, h);
            xcoordinate = xcoordinate + 75 + 75;
        }


        /// calling the subroutine of the Stars to become five stars
        ///int o = 6;
        xcoordinate = -350;
        ycoordinate = 240;
        for (int i = 1; i < s; i++)
        {
            Stars(this, xcoordinate, ycoordinate, w, h);
            xcoordinate = xcoordinate + 75 + 75;
        }


        /// calling the subroutine of the Stars to become five stars 
        ///int p = 6;
        xcoordinate = -250;
        ycoordinate = 350;
        for (int i = 1; i < s; i++)
        {
            Stars(this, xcoordinate, ycoordinate, w, h);
            xcoordinate = xcoordinate + 75 + 75;
        }

        Buttons();

    }

    
    /// <summary>
    /// the final shape of the game (finish the game)
    /// </summary>
    public void ThirdMap()
    {
        ClearAll();
        Level.Size = Screen.Size;
        // Level.Width = Screen.Width
        // Level.Height = Screen.Height;
        Camera.ZoomToLevel();
        Level.CreateBorders();
        Level.Background.Image = BgImages[2];
        MessageDisplay.Add("Onnittelut!");
        victory.Play();
        MessageDisplay.Position = new Vector(450, 500);
        ///Level.Background.Color = Color.Red;

        Buttons();
    }
}