const category: {
  id: number
  name: string
}[] = []

for (let i = 0; i < 20; i++) {
  category.push({
    id: i,
    name: `category${i}`
  })
}

export { category }
