Public Class FormFinal
    Dim conexion As New Conexion
    Private Sub FormFinal_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarTodo()
        editarCols()
    End Sub

    Private Sub editarCols()
        dgvVentas.Columns(1).Visible = False
        dgvVentas.Columns(7).Visible = False
    End Sub

    Private Sub CargarTodo()
        Conexion.Conectar()
        Dim MiDataRow As DataRow
        MiDataRow = FormPedidos.MiDataSet.Tables("Ventas").Rows(0)

        dgvVentas.DataSource = FormPedidos.MiDataSet
        dgvVentas.DataMember = "Ventas"
    End Sub

End Class