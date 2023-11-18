const major: {
  id: number
  name: string
}[] = []

const subject: {
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
  major.push({
    id: i,
    name: `major${i}`
  })

  subject.push({
    id: i,
    name: `subject${i}`
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

export { major, subject, lecturer, student }
