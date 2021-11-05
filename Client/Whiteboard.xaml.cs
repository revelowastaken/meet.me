﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Interaction logic for Whiteboard.xaml
    /// </summary>
    public partial class WhiteBoardView : Window
    {
        private Button activeMainToolbarButton;
        private Button activeSelectToolbarButton;
        private WhiteBoardViewModel viewModel;
        public Canvas GlobCanvas;
        private int mouseLeftBtnMoveFlag = 0;

        //Button Dynamic Colors 
        private string buttonDefaultColor = "#D500F9";
        private string buttonSelectedColor = "#007C9C";

        //Canvas BG available Colors 
        private string canvasBg1 = "#FFFFFF";
        private string canvasBg2 = "#FF0000";
        private string canvasBg3 = "#00FF00";
        private string canvasBg4 = "#0000FF";
        private string canvasBg5 = "#FFFF00";

        public WhiteBoardView()
        {
            InitializeComponent();
            this.GlobCanvas = MyCanvas;
            viewModel = new WhiteBoardViewModel(GlobCanvas);
        }

        // Canvas Mouse actions 
        private void OnCanvasMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            mouseLeftBtnMoveFlag = 0;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //MessageBox.Show(viewModel.GetActiveTool().ToString());
                switch (viewModel.GetActiveTool())
                {
                    case (WhiteBoardViewModel.WBTools.FreeHand):
                        break;
                    case (WhiteBoardViewModel.WBTools.NewLine):
                        break;
                    case (WhiteBoardViewModel.WBTools.NewRectangle):
                        break;
                    case (WhiteBoardViewModel.WBTools.NewEllipse):
                        break;
                    case (WhiteBoardViewModel.WBTools.Selection):

                        //sets the starting point for usage in TranslateShape/RotateShape
                        this.viewModel.start = e.GetPosition(MyCanvas);
                        break;
                        
                }
            }
        }

        private void OnCanvasMouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                //If mouse was moved after pressing right click and then released, then move/rotate operations would be executed WITHOUT unselecting any shape
                if (mouseLeftBtnMoveFlag == 1)
                {
                    switch (viewModel.GetActiveTool())
                    {
                        case (WhiteBoardViewModel.WBTools.Selection):

                            //MessageBox.Show( this.viewModel.end.X.ToString(), this.viewModel.end.Y.ToString());
                            //MessageBox.Show(this.viewModel.start.X.ToString(), this.viewModel.start.Y.ToString() );

                            //If mouse has actually moved between press and release of left click, the selected shapes are either moved or rotated
                            //FACT : e.GetPosition() gives (0,0) during MouseUp if the position of cursor when MouseDown is the same, hence, the below condition works as intended logically 
                            if (this.viewModel.end.X != 0 && this.viewModel.end.Y != 0)
                            {
                                //sets the end point for usage in both TranslateShape/RotateShape when left mouse button is release
                                this.viewModel.end = e.GetPosition(MyCanvas);

                                //MessageBox.Show(this.viewModel.start.ToString(), this.viewModel.end.ToString());

                                if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
                                {
                                    this.viewModel.shapeManager.RotateShape(GlobCanvas, viewModel.WBOps, viewModel.start, viewModel.end, new List<string>(viewModel.shapeManager.selectedShapes), true);
                                }
                                else
                                {
                                    this.viewModel.shapeManager.MoveShape(GlobCanvas, viewModel.WBOps, viewModel.start, viewModel.end, new List<string>(viewModel.shapeManager.selectedShapes), true);
                                }
                            }
                            //Resetting the value of 'start' to perform the next Move functions
                            this.viewModel.start = e.GetPosition(MyCanvas);
                            break;
                    }
                }
                //If mouse was not moved after left clicking, then shapes would be selected/unselected
                else
                {
                    //MessageBox.Show(viewModel.GetActiveTool().ToString());
                    switch (viewModel.GetActiveTool())
                    {
                        case (WhiteBoardViewModel.WBTools.FreeHand):
                            break;
                        case (WhiteBoardViewModel.WBTools.NewLine):
                            break;
                        case (WhiteBoardViewModel.WBTools.NewRectangle):
                            break;
                        case (WhiteBoardViewModel.WBTools.NewEllipse):
                            break;
                        case (WhiteBoardViewModel.WBTools.Selection):

                            //sets the starting point for usage in TranslateShape/RotateShape
                            this.viewModel.start = e.GetPosition(MyCanvas);

                            //MessageBox.Show(this.viewModel.start.ToString());

                            //IF-ELSE to handle Select operations, i.e, Single & Multi select.
                            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                            {
                                if (e.OriginalSource is Shape)
                                {
                                    //MessageBox.Show("Shape Found");
                                    //Create Shape -> Creates a temp Rectangle for bounding box with height and width same as selected shape
                                    //Add this Shape to selected shape
                                    Shape selectedShape = e.OriginalSource as Shape;

                                    //this.SelectionBox.Visibility = Visibility.Visible;
                                    //GlobCanvas = viewModel.shapeManager.CreateShape(GlobCanvas,viewModel.WBOps, WhiteBoardViewModel.WBTools.NewRectangle,...) 

                                    GlobCanvas = viewModel.shapeManager.SelectShape(GlobCanvas, selectedShape, viewModel.WBOps, 1);
                                }
                                else
                                {
                                    //MessageBox.Show("Entered Else");
                                    GlobCanvas = viewModel.shapeManager.UnselectAllBB(GlobCanvas, viewModel.WBOps);
                                    //this.SelectionBox.Visibility = Visibility.Collapsed;
                                }
                            }
                            else
                            {
                                if (e.OriginalSource is Shape)
                                {
                                    //MessageBox.Show("Shape Found");
                                    //Create Shape -> Creates a temp Rectangle for bounding box with height and width same as selected shape
                                    //Add this Shape to selected shape
                                    Shape selectedShape = e.OriginalSource as Shape;

                                    //this.SelectionBox.Visibility = Visibility.Visible;
                                    //GlobCanvas = viewModel.shapeManager.CreateShape(GlobCanvas,viewModel.WBOps, WhiteBoardViewModel.WBTools.NewRectangle,...) 

                                    GlobCanvas = viewModel.shapeManager.SelectShape(GlobCanvas, selectedShape, viewModel.WBOps, 0);
                                }
                                else
                                {
                                    //MessageBox.Show("Entered Else");
                                    GlobCanvas = viewModel.shapeManager.UnselectAllBB(GlobCanvas, viewModel.WBOps);
                                    //this.SelectionBox.Visibility = Visibility.Collapsed;
                                }
                            }
                            break;
                        case (WhiteBoardViewModel.WBTools.Eraser):
                            break;
                    }

                }
            }
            //Resetting the flag for next usage
            mouseLeftBtnMoveFlag = 0;
            return;
        }

        private void OnCanvasMouseMove(object sender, MouseEventArgs e)
        {
            //MessageBox.Show("Canvas Mouse Move triggered");
            if (e.LeftButton == MouseButtonState.Pressed )
            {
                mouseLeftBtnMoveFlag = 1;
                switch (viewModel.GetActiveTool())
                {
                    case (WhiteBoardViewModel.WBTools.Selection):
                        //sets the end point for usage in TranslateShape/RotateShape
                        this.viewModel.end = e.GetPosition(MyCanvas);

                            if (this.viewModel.end.X != this.viewModel.start.X || this.viewModel.end.Y != this.viewModel.start.Y)
                        //if (this.viewModel.end.X != 0 && this.viewModel.end.Y != 0)
                        {                           
                            //MessageBox.Show(this.viewModel.start.ToString(), this.viewModel.end.ToString());
                            if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
                            {                               
                                this.viewModel.shapeManager.RotateShape(GlobCanvas, viewModel.WBOps, viewModel.start, viewModel.end, new List<string>(viewModel.shapeManager.selectedShapes), false);
                            }

                            else
                            {                               
                                this.viewModel.shapeManager.MoveShape(GlobCanvas, viewModel.WBOps, viewModel.start, viewModel.end, new List<string> (viewModel.shapeManager.selectedShapes) , false);
                                //Resetting the value of 'start' to perform the next Move functions
                                this.viewModel.start = e.GetPosition(MyCanvas);
                            }
                        }
                        
                        break;
                }
            }
            return;
        }

        private void OpenPopupButton_MouseEnter(object sender, MouseEventArgs e)
        {
            SetBGButtonPopUp.StaysOpen = true;
        }

        private void OpenPopupButton_MouseLeave(object sender, MouseEventArgs e)
        {
            SetBGButtonPopUp.StaysOpen = false;
        }

        private void OpenPopupRestoreButton_MouseEnter(object sender, MouseEventArgs e)
        {
            RestoreFramePopUp.StaysOpen = true;
        }

        private void OpenPopupRestoreButton_MouseLeave(object sender, MouseEventArgs e)
        {
            RestoreFramePopUp.StaysOpen = false;
        }

        //Main Toolbar Here 
        //Toolbar selection tool 
        private void ClickedSelectTool(object sender, RoutedEventArgs e)
        {
            if (activeMainToolbarButton != null)
            {
                activeMainToolbarButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(buttonDefaultColor));
                activeMainToolbarButton.ClearValue(Button.BackgroundProperty);
            }

            if (this.SelectToolBar.Visibility == Visibility.Collapsed)
            {
                this.SelectToolBar.Visibility = Visibility.Visible;
            }

            activeMainToolbarButton = sender as Button;
            activeMainToolbarButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(buttonSelectedColor));
            viewModel.ChangeActiveTool(activeMainToolbarButton.Name);
            return;
        }

        //Toolbar Rectangle tool
        private void ClickedRectTool(object sender, RoutedEventArgs e)
        {
            if (activeMainToolbarButton != null)
            {
                activeMainToolbarButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(buttonDefaultColor));
                activeMainToolbarButton.ClearValue(Button.BackgroundProperty);
            }

            if (this.SelectToolBar.Visibility == Visibility.Visible)
            {
                this.SelectToolBar.Visibility = Visibility.Collapsed;
            }

            activeMainToolbarButton = sender as Button;
            activeMainToolbarButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(buttonSelectedColor));
            viewModel.ChangeActiveTool(activeMainToolbarButton.Name);
            return;
        }

        //Toolbar Ellipse tool
        private void ClickedEllTool(object sender, RoutedEventArgs e)
        {
            if (activeMainToolbarButton != null)
            {
                activeMainToolbarButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(buttonDefaultColor));
                activeMainToolbarButton.ClearValue(Button.BackgroundProperty);
            }

            if (this.SelectToolBar.Visibility == Visibility.Visible)
            {
                this.SelectToolBar.Visibility = Visibility.Collapsed;
            }

            activeMainToolbarButton = sender as Button;
            activeMainToolbarButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(buttonSelectedColor));
            viewModel.ChangeActiveTool(activeMainToolbarButton.Name);
            return;
        }

        //Toolbar FreeHand tool
        private void ClickedFreehandTool(object sender, RoutedEventArgs e)
        {
            if (activeMainToolbarButton != null)
            {
                activeMainToolbarButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(buttonDefaultColor));
                activeMainToolbarButton.ClearValue(Button.BackgroundProperty);
            }

            if (this.SelectToolBar.Visibility == Visibility.Visible)
            {
                this.SelectToolBar.Visibility = Visibility.Collapsed;
            }

            activeMainToolbarButton = sender as Button;
            activeMainToolbarButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(buttonSelectedColor));
            viewModel.ChangeActiveTool(activeMainToolbarButton.Name);
            return;
        }

        //Toolbar Eraser tool
        private void ClickedEraserTool(object sender, RoutedEventArgs e)
        {
            if (activeMainToolbarButton != null)
            {
                activeMainToolbarButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(buttonDefaultColor));
                activeMainToolbarButton.ClearValue(Button.BackgroundProperty);
            }

            if (this.SelectToolBar.Visibility == Visibility.Visible)
            {
                this.SelectToolBar.Visibility = Visibility.Collapsed;
            }

            activeMainToolbarButton = sender as Button;
            activeMainToolbarButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(buttonSelectedColor));
            viewModel.ChangeActiveTool(activeMainToolbarButton.Name);
            return;
        }

        //Toolbar Line tool
        private void ClickedLineTool(object sender, RoutedEventArgs e)
        {
            if (activeMainToolbarButton != null)
            {
                activeMainToolbarButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(buttonDefaultColor));
                activeMainToolbarButton.ClearValue(Button.BackgroundProperty);
            }

            if (this.SelectToolBar.Visibility == Visibility.Visible)
            {
                this.SelectToolBar.Visibility = Visibility.Collapsed;
            }

            activeMainToolbarButton = sender as Button;
            activeMainToolbarButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(buttonSelectedColor));
            viewModel.ChangeActiveTool(activeMainToolbarButton.Name);
            return;
        }

        //Selection Toolbar
        //Fill Border Tool Button
        private void ClickedFillBorderTool(object sender, RoutedEventArgs e)
        {
            activeSelectToolbarButton = sender as Button;
            activeSelectToolbarButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(buttonSelectedColor));
            activeSelectToolbarButton.ClearValue(Button.BackgroundProperty);

            //viewModel.ChangeActiveTool(activeMainToolbarButton.Name);
            return;
        }

        //Fill Shape Tool Button
        private void ClickedFillShapeTool(object sender, RoutedEventArgs e)
        {
            activeSelectToolbarButton = sender as Button;
            activeSelectToolbarButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(buttonSelectedColor));
            activeSelectToolbarButton.ClearValue(Button.BackgroundProperty);
            //viewModel.ChangeActiveTool(activeMainToolbarButton.Name);
            //viewModel.shapeManager.SetBackgroundColor();
            return;
        }

        //Duplicate Shape Tool Button
        private void ClickedDuplicateTool(object sender, RoutedEventArgs e)
        {
            activeSelectToolbarButton = sender as Button;
            activeSelectToolbarButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(buttonSelectedColor));
            activeSelectToolbarButton.ClearValue(Button.BackgroundProperty);
            //viewModel.ChangeActiveTool(activeMainToolbarButton.Name);
            return;
        }

        //Delete Shape Tool Button
        private void ClickedDeleteTool(object sender, RoutedEventArgs e)
        {
            activeSelectToolbarButton = sender as Button;
            activeSelectToolbarButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(buttonSelectedColor));
            activeSelectToolbarButton.ClearValue(Button.BackgroundProperty);
            //viewModel.ChangeActiveTool(activeMainToolbarButton.Name);
            GlobCanvas = viewModel.shapeManager.DeleteSelectedShapes(GlobCanvas, viewModel.WBOps);
            return;
        }


        //Radio Button (Set Background)
        private void ColorBtn1Checked(object sender, RoutedEventArgs e)
        {
            GlobCanvas = viewModel.ChangeWbBackground(GlobCanvas, canvasBg1);
        }

        private void ColorBtn2Checked(object sender, RoutedEventArgs e)
        {
            GlobCanvas = viewModel.ChangeWbBackground(GlobCanvas, canvasBg2);
        }

        private void ColorBtn3Checked(object sender, RoutedEventArgs e)
        {
            GlobCanvas = viewModel.ChangeWbBackground(GlobCanvas, canvasBg3);
        }

        private void ColorBtn4Checked(object sender, RoutedEventArgs e)
        {
            GlobCanvas = viewModel.ChangeWbBackground(GlobCanvas, canvasBg4);
        }

        private void ColorBtn5Checked(object sender, RoutedEventArgs e)
        {
            GlobCanvas = viewModel.ChangeWbBackground(GlobCanvas, canvasBg5);
        }


        //Whiteboard General tools 
        private void ClickedClearFrame(object sender, RoutedEventArgs e)
        {
            GlobCanvas.Children.Clear();
            return;
        }

        private void ClickedSaveFrame(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("ClickedSaveFrame");
            return;
        }

        private void ClickedUndoButton(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("ClickedUndo");
            return;
        }

        private void ClickedRedoButton(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("ClickedRedo");
            return;
        }

        private void Bu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Bu.Toggled1 == true)
            {
                MessageBox.Show("Toggled On");
            }
            else
            {
                MessageBox.Show("Toggled Off");
            }
        }

        //Parent Window click event
        /*private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }*/
    }
}