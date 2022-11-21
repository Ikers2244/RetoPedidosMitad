Imports System.Data.SqlClient
Imports System.Net.Security
Imports System.Runtime.InteropServices
Imports FontAwesome.Sharp

Public Class FormPedidos

    Private btnPanel As IconButton
    Private nuevoBoton As Panel
    Private formHijoActual As Form
    Public conexion As New Conexion
    Public MiDataSet As DataSet
    Dim cant As Integer

    Public id As Integer

    Private Sub FormPedidos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarTodo()
        btnAñadir.Visible = True
        btnEliminar.Visible = False
        btnFin.Visible = True
        lblNombre.Visible = True
        lblTotal.Visible = True
    End Sub


    Public Sub New()

        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()

        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().
        nuevoBoton = New Panel()
        nuevoBoton.Size = New Size(7, 104)
        PanelMenu.Controls.Add(nuevoBoton)

        Me.Text = String.Empty
        Me.ControlBox = False
        Me.DoubleBuffered = True
        Me.MaximizedBounds = Screen.PrimaryScreen.WorkingArea
        WindowState = FormWindowState.Maximized
    End Sub
    Private Sub ActivateButton(senderbtn As Object, customColor As Color)
        If senderbtn IsNot Nothing Then
            disableButton()
            btnPanel = CType(senderbtn, IconButton)
            btnPanel.BackColor = Color.MediumSlateBlue
            btnPanel.ForeColor = customColor
            btnPanel.IconColor = customColor
            btnPanel.TextAlign = ContentAlignment.MiddleCenter
            btnPanel.ImageAlign = ContentAlignment.MiddleRight
            btnPanel.TextImageRelation = TextImageRelation.TextBeforeImage

            nuevoBoton.BackColor = customColor
            nuevoBoton.Location = New Point(0, btnPanel.Location.Y)
            nuevoBoton.Visible = True
            nuevoBoton.BringToFront()

        End If
    End Sub

    Private Sub disableButton()
        If btnPanel IsNot Nothing Then
            btnPanel.BackColor = Color.SkyBlue
            btnPanel.ForeColor = Color.Black
            btnPanel.IconColor = Color.Black
            btnPanel.TextAlign = ContentAlignment.MiddleLeft
            btnPanel.ImageAlign = ContentAlignment.MiddleLeft
            btnPanel.TextImageRelation = TextImageRelation.ImageBeforeText
        End If
    End Sub

    Private Sub OpenChildForm(formHijo As Form)
        If formHijoActual IsNot Nothing Then
            formHijoActual.Close()
        End If
        formHijoActual = formHijo

        formHijo.TopLevel = False
        formHijo.FormBorderStyle = FormBorderStyle.None
        formHijo.Dock = DockStyle.Fill
        PanelPrincipal.Controls.Add(formHijo)
        PanelPrincipal.Tag = formHijo
        formHijo.BringToFront()
        formHijo.Show()
        lblTitulo.Text = formHijo.Text
    End Sub

    Private Sub ibPrincipal_Click(sender As Object, e As EventArgs) Handles ibPrincipal.Click
        ActivateButton(sender, Color.SkyBlue)
        OpenChildForm(New FormPrincipal)
    End Sub
    Private Sub ibBebidas_Click(sender As Object, e As EventArgs) Handles ibBebidas.Click
        ActivateButton(sender, Color.SkyBlue)
        OpenChildForm(New FormBebidas)
    End Sub

    Private Sub ibEntrantes_Click(sender As Object, e As EventArgs) Handles ibEntrantes.Click
        ActivateButton(sender, Color.SkyBlue)
        OpenChildForm(New FormEntrantes)
    End Sub

    <DllImport("user32.DLL”, EntryPoint:="ReleaseCapture")>
    Private Shared Sub ReleaseCapure()
    End Sub
    <DllImport("user32.DLL", EntryPoint:="SendMessage")>
    Private Shared Sub SendMessage(ByVal Wind As System.IntPtr, ByVal wii As Integer, ByVal waram As Integer, ByVal Param As Integer)
    End Sub

    Private Sub btnAñadir_Click(sender As Object, e As EventArgs) Handles btnAñadir.Click
        Dim MiDataAdapter As SqlDataAdapter

        MiDataAdapter = New SqlDataAdapter("Select IIF(max(IdPedido) Is Null, 0, max(IdPedido)) as IdPedido From Ventas WHERE Eliminado= 'False'", conexion.MiConexion)
        MiDataSet = New DataSet
        MiDataAdapter.Fill(MiDataSet)

        Dim miDataRow As DataRow
        miDataRow = conexion.MiDataSet.Tables("Ventas").NewRow
        miDataRow("IdPedido") = CInt(MiDataSet.Tables(0).Rows(0).Item("IdPedido") + 1)
        miDataRow("IdEmpleado") = Login.user("IdEmpleado")
        If FormBebidas.Visible = True Then
            miDataRow("IdArticulo") = FormBebidas.idBebidas
        ElseIf FormPrincipal.Visible = True Then
            miDataRow("IdArticulo") = FormPrincipal.dgvPrincipal.SelectedCells.Item(0).Value
        ElseIf FormEntrantes.Visible = True Then
            miDataRow("IdArticulo") = FormEntrantes.dgvEntrantes.SelectedCells.Item(0).Value
        End If

        miDataRow("Fecha") = Today.Date
        miDataRow("Cantidad") = cant
        Dim precioTotal As Double

        If FormBebidas.Visible = True Then
            precioTotal = FormBebidas.precioBebidas
        ElseIf FormPrincipal.Visible = True Then
            precioTotal = FormPrincipal.dgvPrincipal.SelectedCells.Item(3).Value
        ElseIf FormEntrantes.Visible = True Then
            precioTotal = FormEntrantes.dgvEntrantes.SelectedCells.Item(3).Value
        End If
        miDataRow("PrecioTotal") = cant * precioTotal

        conexion.MiDataSet.Tables("Ventas").Rows.Add(miDataRow)
        conexion.MiDataAdapter3.Update(conexion.MiDataSet, "Ventas")
        CargarTodo()
    End Sub

    Private Sub btnFin_Click(sender As Object, e As EventArgs) Handles btnFin.Click



        OpenChildForm(New FormFinal)
        btnAñadir.Visible = False
        btnEliminar.Visible = True
        btnFin.Visible = False
        lblNombre.Visible = False
        lblTotal.Visible = False
    End Sub
    Private Sub CargarTodo()
        conexion.Conectar()
        conexion.CargarDatosArticulos()

        FormFinal.dgvVentas.DataSource = conexion.MiDataSet
        FormFinal.dgvVentas.DataMember = "Ventas"
    End Sub

    Private Sub btnSumar_Click(sender As Object, e As EventArgs) Handles btnSumar.Click
        cant = lblCantidad.Text + 1
        lblCantidad.Text = cant
    End Sub

    Private Sub btnRestar_Click(sender As Object, e As EventArgs) Handles btnRestar.Click
        If lblCantidad.Text.Equals("0") = False Then
            cant = lblCantidad.Text - 1
            lblCantidad.Text = cant
        End If
    End Sub
End Class