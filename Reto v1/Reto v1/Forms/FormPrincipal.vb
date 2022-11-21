Public Class FormPrincipal
    Public conexion As New Conexion

    Private Sub FormPrincipal_Load(sender As Object, e As EventArgs) Handles Me.Load
        CargarTodo()
        editarCols()
    End Sub

    Public Sub New()

        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()

        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

        Me.ControlBox = False
        Me.DoubleBuffered = True
        Me.MaximizedBounds = Screen.PrimaryScreen.WorkingArea
        WindowState = FormWindowState.Maximized
    End Sub

    Private Sub dgvPrincipal_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvPrincipal.CellContentClick

    End Sub

    Private Sub editarCols()
        dgvPrincipal.Columns(0).Width = 500
    End Sub

    Private Sub CargarTodo()
        conexion.Conectar()
        Dim MiDataRow As DataRow
        MiDataRow = conexion.MiDataSet4.Tables("Articulos").Rows(0)

        dgvPrincipal.DataSource = conexion.MiDataSet4
        dgvPrincipal.DataMember = "Articulos"
    End Sub
End Class