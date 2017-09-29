﻿/*************************************************************************************

   Extended WPF Toolkit

   Copyright (C) 2007-2013 Xceed Software Inc.

   This program is provided to you under the terms of the Microsoft Public
   License (Ms-PL) as published at http://wpftoolkit.codeplex.com/license 

   For more features, controls, and fast professional support,
   pick up the Plus Edition at http://xceed.com/wpf_toolkit

   Stay informed: follow @datagrid on Twitter or Like http://facebook.com/datagrids

  ***********************************************************************************/

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Xceed.Wpf.AvalonDock.Layout;

namespace Xceed.Wpf.AvalonDock.Controls
{
  public class LayoutDocumentControl : Control
  {
    static LayoutDocumentControl()
    {
      DefaultStyleKeyProperty.OverrideMetadata( typeof( LayoutDocumentControl ), new FrameworkPropertyMetadata( typeof( LayoutDocumentControl ) ) );
      FocusableProperty.OverrideMetadata( typeof( LayoutDocumentControl ), new FrameworkPropertyMetadata( true ) );
    }

    #region Model

    /// <summary>
    /// Model Dependency Property
    /// </summary>
    public static readonly DependencyProperty ModelProperty = DependencyProperty.Register( "Model",
                                                                                          typeof( LayoutContent ),
                                                                                          typeof( LayoutDocumentControl ),
                                                                                          new FrameworkPropertyMetadata( null, OnModelChanged ) );

    /// <summary>
    /// Gets or sets the Model property.  This dependency property 
    /// indicates the model attached to this view.
    /// </summary>
    public LayoutContent Model
    {
      get
      {
        return ( LayoutContent )GetValue( ModelProperty );
      }
      set
      {
        SetValue( ModelProperty, value );
      }
    }

    /// <summary>
    /// Handles changes to the Model property.
    /// </summary>
    private static void OnModelChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
    {
      ( ( LayoutDocumentControl )d ).OnModelChanged( e );
    }

    /// <summary>
    /// Provides derived classes an opportunity to handle changes to the Model property.
    /// </summary>
    protected virtual void OnModelChanged( DependencyPropertyChangedEventArgs e )
    {
      if( e.OldValue != null )
      {
        ( ( LayoutContent )e.OldValue ).PropertyChanged -= Model_PropertyChanged;
      }

      if( Model != null )
      {
        Model.PropertyChanged += Model_PropertyChanged;
        SetLayoutItem( Model.Root.Manager.GetLayoutItemFromModel( Model ) );
      }
      else
      {
        SetLayoutItem( null );
      }
    }

    private void Model_PropertyChanged( object sender, PropertyChangedEventArgs e )
    {
      if( e.PropertyName == "IsEnabled" )
      {
        if( Model != null )
        {
          IsEnabled = Model.IsEnabled;
          if( !IsEnabled && Model.IsActive )
          {
            if( ( Model.Parent != null ) && ( Model.Parent is LayoutDocumentPane ) )
            {
              ( ( LayoutDocumentPane )Model.Parent ).SetNextSelectedIndex();
            }
          }
        }
      }
    }

    #endregion

    #region LayoutItem

    /// <summary>
    /// LayoutItem Read-Only Dependency Property
    /// </summary>
    private static readonly DependencyPropertyKey LayoutItemPropertyKey = DependencyProperty.RegisterReadOnly( "LayoutItem",
                                                                                                              typeof( LayoutItem ),
                                                                                                              typeof( LayoutDocumentControl ),
                                                                                                              new FrameworkPropertyMetadata(
                                                                                                                  ( LayoutItem )null ) );

    public static readonly DependencyProperty LayoutItemProperty = LayoutItemPropertyKey.DependencyProperty;

    /// <summary>
    /// Gets the LayoutItem property.  This dependency property 
    /// indicates the LayoutItem attached to this tag item.
    /// </summary>
    public LayoutItem LayoutItem
    {
      get
      {
        return ( LayoutItem )GetValue( LayoutItemProperty );
      }
    }

    /// <summary>
    /// Provides a secure method for setting the LayoutItem property.  
    /// This dependency property indicates the LayoutItem attached to this tag item.
    /// </summary>
    /// <param name="value">The new value for the property.</param>
    protected void SetLayoutItem( LayoutItem value )
    {
      SetValue( LayoutItemPropertyKey, value );
    }

    #endregion

    protected override void OnPreviewGotKeyboardFocus( KeyboardFocusChangedEventArgs e )
    {
      this.SetIsActive();
      base.OnPreviewGotKeyboardFocus( e );
    }

    protected override void OnMouseLeftButtonDown( MouseButtonEventArgs e )
    {
      this.SetIsActive();
      base.OnMouseLeftButtonDown( e );
    }

    protected override void OnMouseRightButtonDown( MouseButtonEventArgs e )
    {
      this.SetIsActive();
      base.OnMouseLeftButtonDown( e );
    }

    private void SetIsActive()
    {
      if( this.Model != null )
      {
        this.Model.IsActive = true;
      }
    }
  }
}
