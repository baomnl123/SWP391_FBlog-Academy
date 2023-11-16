const category: {
  id: number
  name: string
}[] = []

const tag: {
  id: number
  name: string
}[] = []

const lecturer: {
  id: number
  name: string
}[] = []

const student: {
  id: number
  name: string
}[] = []

for (let i = 0; i < 20; i++) {
  category.push({
    id: i,
    name: `category${i}`
  })

  tag.push({
    id: i,
    name: `tag${i}`
  })

  lecturer.push({
    id: i,
    name: `lecturer${i}`
  })

  student.push({
    id: i,
    name: `student${i}`
  })
}

export { category, tag, lecturer, student }
