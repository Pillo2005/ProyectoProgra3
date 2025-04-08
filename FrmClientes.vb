Imports System.Data.SqlClient
Public Class FrmClientes



    Dim conBD As Conexion = New Conexion()
    Private Sub FrmClientes_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MostrarClientes()

    End Sub

    Private Sub MostrarClientes()
        Dim consulta As String = "SELECT * FROM Clientes"
        Dim adaptador As New SqlDataAdapter(consulta, conBD.Conectar())
        Dim tabla As New DataTable()
        adaptador.Fill(tabla)
        dgvClientes.DataSource = tabla
        conBD.Cerrar()
    End Sub

    Private Sub LimpiarCampos()
        txtIdCliente.Text = ""
        txtNombre.Text = ""
        txtTelefono.Text = ""
        txtEmail.Text = ""
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim consulta As String = "INSERT INTO Clientes (Nombre, Telefono, Correo) VALUES (@nombre, @telefono, @email)"
        Dim comando As New SqlCommand(consulta, conBD.Conectar())
        comando.Parameters.AddWithValue("@nombre", txtNombre.Text)
        comando.Parameters.AddWithValue("@telefono", txtTelefono.Text)
        comando.Parameters.AddWithValue("@email", txtEmail.Text)
        comando.ExecuteNonQuery()
        conBD.Cerrar()

        MostrarClientes()
        LimpiarCampos()
        MsgBox("Cliente guardado correctamente", MsgBoxStyle.Information)
    End Sub

    Private Sub btnActualizar_Click(sender As Object, e As EventArgs) Handles btnActualizar.Click

        If txtIdCliente.Text <> "" Then
            Dim consulta As String = "UPDATE Clientes SET Nombre=@nombre, Telefono=@telefono, Correo=@email WHERE IdCliente=@id"

            Dim comando As New SqlCommand(consulta, conBD.Conectar())
            comando.Parameters.AddWithValue("@id", Convert.ToInt32(txtIdCliente.Text))
            comando.Parameters.AddWithValue("@nombre", txtNombre.Text)
            comando.Parameters.AddWithValue("@telefono", txtTelefono.Text)
            comando.Parameters.AddWithValue("@email", txtEmail.Text)
            comando.ExecuteNonQuery()
            conBD.Cerrar()

            MostrarClientes()
            LimpiarCampos()
            MsgBox("Cliente actualizado correctamente", MsgBoxStyle.Information)
        End If

    End Sub

    Private Sub dgvClientes_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvClientes.CellContentClick

        If e.RowIndex >= 0 Then
            Dim fila As DataGridViewRow = dgvClientes.Rows(e.RowIndex)
            txtIdCliente.Text = fila.Cells("IdCliente").Value.ToString()
            txtNombre.Text = fila.Cells("Nombre").Value.ToString()
            txtTelefono.Text = fila.Cells("Telefono").Value.ToString()
            txtEmail.Text = fila.Cells("Email").Value.ToString()
        End If

    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If txtIdCliente.Text <> "" Then
            Dim resultado = MsgBox("¿Desea eliminar este cliente?", MsgBoxStyle.YesNo)
            If resultado = MsgBoxResult.Yes Then

                Dim consulta As String = "DELETE FROM Clientes WHERE IdCliente=@id"

                Dim comando As New SqlCommand(consulta, conBD.Conectar())
                comando.Parameters.AddWithValue("@id", Convert.ToInt32(txtIdCliente.Text))
                comando.ExecuteNonQuery()
                conBD.Cerrar()

                MostrarClientes()
                LimpiarCampos()
                MsgBox("Cliente eliminado correctamente", MsgBoxStyle.Information)
            End If
        End If
    End Sub

    Private Sub btnLimpiar_Click(sender As Object, e As EventArgs) Handles btnLimpiar.Click
        LimpiarCampos()
    End Sub

    Private Sub btnHistorial_Click(sender As Object, e As EventArgs) Handles btnHistorial.Click
        If txtIdCliente.Text <> "" Then
            Dim historial As New lblBienvenida

            historial.IdClienteSeleccionado = Convert.ToInt32(txtIdCliente.Text)
            historial.NombreCliente = txtNombre.Text
            historial.ShowDialog()
        Else
            MsgBox("Selecciona un cliente primero.", MsgBoxStyle.Exclamation)
        End If
    End Sub

End Class