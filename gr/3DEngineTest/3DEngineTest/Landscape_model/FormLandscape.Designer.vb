﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormLandscape
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormLandscape))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LoadModelToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AutoRotateToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RotateXToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RotateYToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RotateZToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ResetToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.LightToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ResetToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TrackBar1 = New System.Windows.Forms.TrackBar()
        Me.RemoveTexturesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1.SuspendLayout()
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(742, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LoadModelToolStripMenuItem, Me.AutoRotateToolStripMenuItem, Me.LightToolStripMenuItem, Me.RemoveTexturesToolStripMenuItem, Me.ResetToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'LoadModelToolStripMenuItem
        '
        Me.LoadModelToolStripMenuItem.Name = "LoadModelToolStripMenuItem"
        Me.LoadModelToolStripMenuItem.Size = New System.Drawing.Size(163, 22)
        Me.LoadModelToolStripMenuItem.Text = "Load Model"
        '
        'AutoRotateToolStripMenuItem
        '
        Me.AutoRotateToolStripMenuItem.Checked = True
        Me.AutoRotateToolStripMenuItem.CheckOnClick = True
        Me.AutoRotateToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.AutoRotateToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RotateXToolStripMenuItem, Me.RotateYToolStripMenuItem, Me.RotateZToolStripMenuItem, Me.ToolStripMenuItem1, Me.ResetToolStripMenuItem1})
        Me.AutoRotateToolStripMenuItem.Name = "AutoRotateToolStripMenuItem"
        Me.AutoRotateToolStripMenuItem.Size = New System.Drawing.Size(163, 22)
        Me.AutoRotateToolStripMenuItem.Text = "Auto Rotate"
        '
        'RotateXToolStripMenuItem
        '
        Me.RotateXToolStripMenuItem.Checked = True
        Me.RotateXToolStripMenuItem.CheckOnClick = True
        Me.RotateXToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.RotateXToolStripMenuItem.Name = "RotateXToolStripMenuItem"
        Me.RotateXToolStripMenuItem.Size = New System.Drawing.Size(115, 22)
        Me.RotateXToolStripMenuItem.Text = "rotate X"
        '
        'RotateYToolStripMenuItem
        '
        Me.RotateYToolStripMenuItem.Checked = True
        Me.RotateYToolStripMenuItem.CheckOnClick = True
        Me.RotateYToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.RotateYToolStripMenuItem.Name = "RotateYToolStripMenuItem"
        Me.RotateYToolStripMenuItem.Size = New System.Drawing.Size(115, 22)
        Me.RotateYToolStripMenuItem.Text = "rotate Y"
        '
        'RotateZToolStripMenuItem
        '
        Me.RotateZToolStripMenuItem.Checked = True
        Me.RotateZToolStripMenuItem.CheckOnClick = True
        Me.RotateZToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.RotateZToolStripMenuItem.Name = "RotateZToolStripMenuItem"
        Me.RotateZToolStripMenuItem.Size = New System.Drawing.Size(115, 22)
        Me.RotateZToolStripMenuItem.Text = "rotate Z"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(112, 6)
        '
        'ResetToolStripMenuItem1
        '
        Me.ResetToolStripMenuItem1.Name = "ResetToolStripMenuItem1"
        Me.ResetToolStripMenuItem1.Size = New System.Drawing.Size(115, 22)
        Me.ResetToolStripMenuItem1.Text = "Reset"
        '
        'LightToolStripMenuItem
        '
        Me.LightToolStripMenuItem.Checked = True
        Me.LightToolStripMenuItem.CheckOnClick = True
        Me.LightToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.LightToolStripMenuItem.Name = "LightToolStripMenuItem"
        Me.LightToolStripMenuItem.Size = New System.Drawing.Size(163, 22)
        Me.LightToolStripMenuItem.Text = "Light"
        '
        'ResetToolStripMenuItem
        '
        Me.ResetToolStripMenuItem.Name = "ResetToolStripMenuItem"
        Me.ResetToolStripMenuItem.Size = New System.Drawing.Size(163, 22)
        Me.ResetToolStripMenuItem.Text = "Reset"
        '
        'TrackBar1
        '
        Me.TrackBar1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TrackBar1.AutoSize = False
        Me.TrackBar1.BackColor = System.Drawing.Color.LightBlue
        Me.TrackBar1.LargeChange = 10
        Me.TrackBar1.Location = New System.Drawing.Point(597, 462)
        Me.TrackBar1.Maximum = -5
        Me.TrackBar1.Minimum = -500
        Me.TrackBar1.Name = "TrackBar1"
        Me.TrackBar1.Size = New System.Drawing.Size(123, 19)
        Me.TrackBar1.SmallChange = 5
        Me.TrackBar1.TabIndex = 1
        Me.TrackBar1.Value = -5
        '
        'RemoveTexturesToolStripMenuItem
        '
        Me.RemoveTexturesToolStripMenuItem.Name = "RemoveTexturesToolStripMenuItem"
        Me.RemoveTexturesToolStripMenuItem.Size = New System.Drawing.Size(163, 22)
        Me.RemoveTexturesToolStripMenuItem.Text = "Remove Textures"
        '
        'FormLandscape
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(742, 493)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.TrackBar1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "FormLandscape"
        Me.Text = "VB.NET 3D graphics engine demo"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LoadModelToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AutoRotateToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ResetToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TrackBar1 As TrackBar
    Friend WithEvents RotateXToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RotateYToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RotateZToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As ToolStripSeparator
    Friend WithEvents ResetToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents LightToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RemoveTexturesToolStripMenuItem As ToolStripMenuItem
End Class
