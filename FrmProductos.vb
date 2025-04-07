Imports System.Data.SqlClient
Public Class FrmProductos
    Dim conBD As Conexion = New Conexion()
    Private Sub FrmProductos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cboCategoria.Items.AddRange(New String() {"Ropa", "Tecnología", "Alimentos", "Accesorios"})
        MostrarProductos()
    End Sub

    Private Sub MostrarProductos()
        Dim consulta As String = "SELECT * FROM Productos"
        Dim adaptador As New SqlDataAdapter(consulta, conBD.Conectar())
        Dim tabla As New DataTable()
        adaptador.Fill(tabla)
        dgvProductos.DataSource = tabla
        conBD.Cerrar()
    End Sub

    Private Sub LimpiarCampos()
        txtIdProducto.Text = ""
        txtNombre.Text = ""
        cboCategoria.SelectedIndex = -1
        txtPrecio.Text = ""
        txtStock.Text = ""
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click

        Dim consulta As String = "INSERT INTO Productos (Nombre, Categoria, Precio, Stock) VALUES (@nombre, @categoria, @precio, @stock)"

        Dim comando As New SqlCommand(consulta, conBD.Conectar())
        comando.Parameters.AddWithValue("@nombre", txtNombre.Text)
        comando.Parameters.AddWithValue("@categoria", cboCategoria.Text)
        comando.Parameters.AddWithValue("@precio", Convert.ToDecimal(txtPrecio.Text))
        comando.Parameters.AddWithValue("@stock", Convert.ToInt32(txtStock.Text))
        comando.ExecuteNonQuery()
        conBD.Cerrar()

        MostrarProductos()
        LimpiarCampos()
        MsgBox("Producto guardado correctamente", MsgBoxStyle.Information)

    End Sub



End Class