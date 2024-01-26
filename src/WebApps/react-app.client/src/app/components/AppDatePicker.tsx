import { DemoContainer } from '@mui/x-date-pickers/internals/demo';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import {useController, UseControllerProps} from "react-hook-form";

interface Props extends UseControllerProps {
    label: string;
}

export default function AppDatePicker(props: Props) {

    const {fieldState, field} = useController({...props, defaultValue: ''})

    return (
        <LocalizationProvider dateAdapter={AdapterDayjs}>
            <DemoContainer components={['DatePicker']}>
                <DatePicker 
                    {...props}
                    {...field}
                    label={props.label} 
                    slotProps={{ textField: { fullWidth: true, error: !!fieldState.error, helperText: fieldState.error?.message }}}
                />
            </DemoContainer>
        </LocalizationProvider>
    );
}
