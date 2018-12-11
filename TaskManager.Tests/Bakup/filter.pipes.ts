import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filter'
})
export class FilterPipe implements PipeTransform {

  transform(value: any, args?: any): any {
    if (args.Project) {
      value = value.filter(proj => proj.Project.toLowerCase().indexOf(args.Project.toLowerCase()) != -1);
    }
    if (args.FirstName) {
      value = value.filter(proj => proj.FirstName.toLowerCase().indexOf(args.FirstName.toLowerCase()) != -1);
    }
    return value;

  }

}
