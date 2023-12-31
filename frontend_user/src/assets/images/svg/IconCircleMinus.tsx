import { BaseImgSvg } from './types'

const IconCircleMinus = ({ color = '#fff', height = 20, width = 20 }: BaseImgSvg) => {
  return (
    <svg xmlns='http://www.w3.org/2000/svg' height={height} width={width} fill={color} viewBox='0 0 512 512'>
      <path d='M256 512A256 256 0 1 0 256 0a256 256 0 1 0 0 512zM184 232H328c13.3 0 24 10.7 24 24s-10.7 24-24 24H184c-13.3 0-24-10.7-24-24s10.7-24 24-24z' />
    </svg>
  )
}

export default IconCircleMinus
