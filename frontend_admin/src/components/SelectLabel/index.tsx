import { Select, SelectProps, Typography } from 'antd'

export interface SelectLabelProps extends SelectProps {
  label?: string
  optionData?: SelectProps['options']
  onChange?: (value: string | string[] | number | number[]) => void
  placeHolder?: string
}

const SelectLabel = ({ label, optionData, placeHolder, onChange, ...props }: SelectLabelProps) => {
  const handleChangeTag = (value: string[]) => {
    onChange?.(value)
  }

  return (
    <div className='mb-6 w-full'>
      <Typography className='mb-2'>{label}</Typography>
      <Select
        mode='multiple'
        style={{ width: '100%' }}
        placeholder={placeHolder}
        onChange={handleChangeTag}
        optionLabelProp='label'
        options={optionData}
        {...props}
      />
    </div>
  )
}

export default SelectLabel
