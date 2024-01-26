import * as yup from 'yup';

export const validationSchema = [
    yup.object({
        fullName: yup.string().required('Full name is required'),
        email: yup.string().required('Email is required'),
        addressLine: yup.string().required('Address line is required'),
        city: yup.string().required(),
        state: yup.string().required(),
        zipCode: yup.string().required(),
        country: yup.string().required(),
    }),
    yup.object(),
    yup.object({
        nameOnCard: yup.string().required(),
        cardNumber: yup.string().required(),
        cardExpiryDate: yup.date().required(),
        cvv: yup.string().required()
    })
]