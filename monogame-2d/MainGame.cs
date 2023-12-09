﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using monogame2d.Enum;
using monogame2d.Objects;
using monogame2d.States;
using monogame2d.States.Base;
namespace monogame_2d;

public class MainGame : Game
{
    private BaseGameState _currentGameState;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    private RenderTarget2D _renderTarget;
    private Rectangle _renderScaleRectangle;
    private const int DESIGNED_RESOLUTION_WIDTH = 640;
    private const int DESIGNED_RESOLUTION_HEIGHT = 480;

    private const float DESIGNED_RESOLUTION_ASPECT_RATIO = DESIGNED_RESOLUTION_WIDTH / (float)DESIGNED_RESOLUTION_HEIGHT;


    public MainGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        // Full Screen support
        _graphics.PreferredBackBufferWidth = 1024;
        _graphics.PreferredBackBufferHeight = 768;
        _graphics.IsFullScreen = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        _renderTarget = new RenderTarget2D(
            _graphics.GraphicsDevice,
            DESIGNED_RESOLUTION_WIDTH,
            DESIGNED_RESOLUTION_HEIGHT,
            false,
            SurfaceFormat.Color,
            DepthFormat.None,
            0,
            RenderTargetUsage.DiscardContents);

        _renderScaleRectangle = GetScaleRectangle();

        base.Initialize();
    }
    // LoadContent will be called once per game and is the place to load
    // all of your content.
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            // Exit();
            Console.WriteLine("Test debug");
        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // TODO: Add your drawing code here

        GraphicsDevice.SetRenderTarget(_renderTarget);
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();
        _currentGameState.Render(_spriteBatch);
        _spriteBatch.End();

        // render the scaled content
        _graphics.GraphicsDevice.SetRenderTarget(null);
        _graphics.GraphicsDevice.Clear(
            ClearOptions.Target,
            Color.Black,
            1.0f,
            0);

        _spriteBatch.Begin(
            SpriteSortMode.Immediate,
            BlendState.Opaque);
        _spriteBatch.Draw(
            _renderTarget,
            _renderScaleRectangle,
            Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private Rectangle GetScaleRectangle()
    {
        var variance = 0.5;
        var actualAspectRatio = Window.ClientBounds.Width / (float)Window.ClientBounds.Height;

        Rectangle scaleRectangle;

        if (actualAspectRatio <= DESIGNED_RESOLUTION_ASPECT_RATIO)
        {
            var presentHeight = (int)(Window.ClientBounds.Width / DESIGNED_RESOLUTION_ASPECT_RATIO + variance);
            var barHeight = (Window.ClientBounds.Height - presentHeight) / 2;
            scaleRectangle = new Rectangle(0, barHeight, Window.ClientBounds.Width, presentHeight);
        }
        else
        {
            var presentWidth = (int)(Window.ClientBounds.Height * DESIGNED_RESOLUTION_ASPECT_RATIO + variance);
            var barWidth = (Window.ClientBounds.Width - presentWidth) / 2;
            scaleRectangle = new Rectangle(barWidth, 0, presentWidth, Window.ClientBounds.Height);
        }

        return scaleRectangle;
    }

    public virtual void OnNotify(Events eventType) { }

    private void SwitchGameState(BaseGameState gameState)
    {
        _currentGameState?.UnloadContent(Content);
        _currentGameState = gameState;
        _currentGameState.LoadContent(Content);
        _currentGameState.OnStateSwitched += CurrentGameState_OnStateSwitched;
        _currentGameState.OnEventNotification += _currentGameState_OnEventNotification;

    }

    private void _currentGameState_OnEventNotification(object sender, Events e)
    {
        switch (e)
        {
            case Events.GAME_QUIT:
                Exit();
                break;
        }
    }

    private void CurrentGameState_OnStateSwitched(object sender, BaseGameState e)
    {
        SwitchGameState(e);
    }
}

