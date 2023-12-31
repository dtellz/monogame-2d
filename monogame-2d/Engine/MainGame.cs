﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using monogame2d.Engine.States;
namespace monogame_2d.Engine;

public class MainGame : Game
{
    private BaseGameState _currentGameState;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    private RenderTarget2D _renderTarget;
    private Rectangle _renderScaleRectangle;

    private int _DesignedResolutionWidth;
    private int _DesignedResolutionHeight;
    private float _DesignedResolutionRatio;

    private BaseGameState _firstGameState;
    // private const int DESIGNED_RESOLUTION_WIDTH = 1280;
    // private const int DESIGNED_RESOLUTION_HEIGHT = 720;

    // private const float DESIGNED_RESOLUTION_ASPECT_RATIO = DESIGNED_RESOLUTION_WIDTH / (float)DESIGNED_RESOLUTION_HEIGHT;


    public MainGame(int width, int height, BaseGameState firstGameState)
    {
        Content.RootDirectory = "Content";
        _graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = width,
            PreferredBackBufferHeight = height,
            IsFullScreen = false,
        };
        _firstGameState = firstGameState;
        _DesignedResolutionWidth = width;
        _DesignedResolutionHeight = height;
        _DesignedResolutionRatio = width / (float)height;

        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _graphics.PreferredBackBufferWidth = 1024;
        _graphics.PreferredBackBufferHeight = 768;
        _graphics.IsFullScreen = false;
        _graphics.ApplyChanges();

        _renderTarget = new RenderTarget2D(_graphics.GraphicsDevice, _DesignedResolutionWidth, _DesignedResolutionHeight, false,
            SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);

        _renderScaleRectangle = GetScaleRectangle();

        base.Initialize();
    }
    // LoadContent will be called once per game and is the place to load
    // all of your content.
    protected override void LoadContent()
    {
        // TODO: use this.Content to load your game content here
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        SwitchGameState(_firstGameState);
    }

    protected override void Update(GameTime gameTime)
    {
        _currentGameState.HandleInput(gameTime);
        _currentGameState.Update(gameTime);
            
        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // Render to the Render Target
        GraphicsDevice.SetRenderTarget(_renderTarget);

        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        if (_currentGameState != null)
        {
            _currentGameState.Render(_spriteBatch);
        }
        _spriteBatch.End();

        // Now render the scaled content
        _graphics.GraphicsDevice.SetRenderTarget(null);

        _graphics.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 1.0f, 0);

        _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);

        _spriteBatch.Draw(_renderTarget, _renderScaleRectangle, Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private Rectangle GetScaleRectangle()
    {
        var variance = 0.5;
        var actualAspectRatio = Window.ClientBounds.Width / (float)Window.ClientBounds.Height;

        Rectangle scaleRectangle;

        if (actualAspectRatio <= _DesignedResolutionRatio)
        {
            var presentHeight = (int)(Window.ClientBounds.Width / _DesignedResolutionRatio + variance);
            var barHeight = (Window.ClientBounds.Height - presentHeight) / 2;

            scaleRectangle = new Rectangle(0, barHeight, Window.ClientBounds.Width, presentHeight);
        }
        else
        {
            var presentWidth = (int)(Window.ClientBounds.Height * _DesignedResolutionRatio + variance);
            var barWidth = (Window.ClientBounds.Width - presentWidth) / 2;

            scaleRectangle = new Rectangle(barWidth, 0, presentWidth, Window.ClientBounds.Height);
        }

        return scaleRectangle;
    }

    public virtual void OnNotify(BaseGameStateEvent eventType) { }

    private void SwitchGameState(BaseGameState gameState)
    {
        _currentGameState?.UnloadContent();
        _currentGameState = gameState;
        _currentGameState.Initialize(Content, _graphics.GraphicsDevice.Viewport.Width, _graphics.GraphicsDevice.Viewport.Height);
        _currentGameState.LoadContent();
        _currentGameState.OnStateSwitched += CurrentGameState_OnStateSwitched;
        _currentGameState.OnEventNotification += _currentGameState_OnEventNotification;

    }

    private void _currentGameState_OnEventNotification(object sender, BaseGameStateEvent e)
    {
        switch (e)
        {
            case BaseGameStateEvent.GameQuit:
                Exit();
                break;
        }
    }

    private void CurrentGameState_OnStateSwitched(object sender, BaseGameState e)
    {
        SwitchGameState(e);
    }
}

