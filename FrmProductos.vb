Imports System.Data.SqlClient
Public Class FrmProductos
    Dim conBD As Conexion = New Conexion()
    Private Sub FrmProductos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cboCategoria.Items.AddRange(New String() {"Ropa", "Tecnología", "Alimentos", "Accesorios"})
        MostrarProductos()
        AlertaStockBajo()
    End Sub

    Private Sub MostrarProductos()
        Dim consulta As String = "SELECT * FROM Productos"
        Dim adaptador As New SqlDataAdapter(consulta, conBD.Conectar())
        Dim tabla As New DataTable()
        adaptador.Fill(tabla)
        dgvProductos.DataSource = tabla
        StockBajoTable()
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

    Private Sub dgvProductos_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvProductos.CellContentClick
        If e.RowIndex >= 0 Then
            Dim fila As DataGridViewRow = dgvProductos.Rows(e.RowIndex)
            txtIdProducto.Text = fila.Cells("IdProducto").Value.ToString()
            txtNombre.Text = fila.Cells("Nombre").Value.ToString()
            cboCategoria.Text = fila.Cells("Categoria").Value.ToString()
            txtPrecio.Text = fila.Cells("Precio").Value.ToString()
            txtStock.Text = fila.Cells("Stock").Value.ToString()

        End If

    End Sub

    Private Sub btnActualizar_Click(sender As Object, e As EventArgs) Handles btnActualizar.Click
        If txtIdProducto.Text <> "" Then
            Dim consulta As String = "UPDATE Productos SET Nombre=@nombre, Categoria=@categoria, Precio=@precio, Stock=@stock WHERE IdProducto=@id"

            Dim comando As New SqlCommand(consulta, conBD.Conectar())
            comando.Parameters.AddWithValue("@id", Convert.ToInt32(txtIdProducto.Text))
            comando.Parameters.AddWithValue("@nombre", txtNombre.Text)
            comando.Parameters.AddWithValue("@categoria", cboCategoria.Text)
            comando.Parameters.AddWithValue("@precio", Convert.ToDecimal(txtPrecio.Text))
            comando.Parameters.AddWithValue("@stock", Convert.ToInt32(txtStock.Text))
            comando.ExecuteNonQuery()
            conBD.Cerrar()
            MostrarProductos()
            LimpiarCampos()
            MsgBox("Producto actualizado correctamente", MsgBoxStyle.Information)
        End If


    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If txtIdProducto.Text <> "" Then
            Dim resultado = MsgBox("¿Está seguro que desea eliminar este producto?", MsgBoxStyle.YesNo)
            If resultado = MsgBoxResult.Yes Then

                Dim consulta As String = "DELETE FROM Productos WHERE IdProducto=@id"

                Dim comando As New SqlCommand(consulta, conBD.Conectar())
                comando.Parameters.AddWithValue("@id", Convert.ToInt32(txtIdProducto.Text))
                comando.ExecuteNonQuery()
                conBD.Cerrar()

                MostrarProductos()
                LimpiarCampos()
                MsgBox("Producto eliminado correctamente", MsgBoxStyle.Information)
            End If
        End If
    End Sub

    Private Sub btnLimpiar_Click(sender As Object, e As EventArgs) Handles btnLimpiar.Click
        LimpiarCampos()
    End Sub


    Private Sub StockBajoTable()
        For Each fila As DataGridViewRow In dgvProductos.Rows
            If Convert.ToInt32(fila.Cells("Stock").Value) <= 5 And Convert.ToInt32(fila.Cells("Stock").Value) >= 1 Then
                fila.DefaultCellStyle.BackColor = Color.LightCoral ' Color rojo claro
            End If
        Next
    End Sub

    Private Sub AlertaStockBajo()
        Dim consulta As String = "SELECT COUNT(*) FROM Productos WHERE Stock <= 5"
        Dim comando As New SqlCommand(consulta, conBD.Conectar())
        Dim cantidadBajos As Integer = Convert.ToInt32(comando.ExecuteScalar())

        If cantidadBajos > 0 Then
            MsgBox("⚠️ Hay " & cantidadBajos & " productos con stock bajo.", MsgBoxStyle.Exclamation, "Alerta de Inventario")
        End If

        conBD.Cerrar()
    End Sub



End Class