import * as Yup from 'yup';

export const loginInitValues = {
    email:'',
    password:'',
}

export const loginValidationSchema = Yup.object({
    email: Yup.string()
    .required('El correo electronico es requerido')
    .email('Ingrese un Correo electronico Valido'),
    password: Yup.string()
    .required('La clave de seguridad es requerida.')
});