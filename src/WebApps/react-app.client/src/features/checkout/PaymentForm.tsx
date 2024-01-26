import { Typography, Grid } from "@mui/material";
import { useFormContext } from "react-hook-form";
import AppTextInput from "../../app/components/AppTextInput";
import AppDatePicker from "../../app/components/AppDatePicker.tsx";

export default function PaymentForm() {
  const { control } = useFormContext();

  return (
    <>
      <Typography variant="h6" gutterBottom>
        Payment method
      </Typography>
      <Grid container spacing={3}>
        <Grid item xs={12} md={6}>
          <AppTextInput
            name='nameOnCard'
            label='Name on card'
            control={control}
          />
        </Grid>
        <Grid item xs={12} md={6}>
          <AppTextInput 
            name='cardNumber'
            control={control}
            label="Card number"
          />
        </Grid>
        <Grid item xs={12} md={6}>
          <AppDatePicker
              name='cardExpiryDate'
              label="Expiry date"
              control={control}
          />
        </Grid>
        <Grid item xs={12} md={6}>
          <AppTextInput
            name='cvv'
            control={control}
            label="CVV"
          />
        </Grid>
      </Grid>
    </>
  );
}