import { ListItem } from "./list-item";

export class ListItemGroup<T> {
  constructor(public label: string, public listItems: Array<ListItem<T>>) {}
}
