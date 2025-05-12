import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Reminder } from '../../models/reminder.model';
import { Store } from '@ngrx/store';
import { loadReminders, updateReminder } from '../../store/reminder/reminder.actions';
import { selectAllReminders } from '../../store/reminder/reminder.selectors';

import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';

@Component({
  selector: 'app-reminder-list',
  templateUrl: './reminder-list.component.html',
  standalone: false
})
export class ReminderListComponent implements OnInit {
  reminders: Reminder[] = [];
  filteredReminders: Reminder[] = [];
  sortOrder: string = 'all';
  editingReminder: Reminder | null = null;

  @ViewChild('editForm') editFormElement?: ElementRef;

  constructor(private store: Store) {}

  ngOnInit() {
    this.store.dispatch(loadReminders());
    this.store.select(selectAllReminders).subscribe((reminders: Reminder[]) => {
      this.reminders = reminders;
      this.sortReminders();
    });
  }

  sortReminders() {
    this.filteredReminders = this.sortOrder === 'done' 
      ? this.reminders.filter(r => r.done) 
      : this.sortOrder === 'not-done' 
      ? this.reminders.filter(r => !r.done) 
      : this.reminders;
  }

  exportToExcel(): void {
    const cleanedData = this.filteredReminders.map(r => ({
      Company: r.companyName,
      Amount: r.amount,
      Description: r.description,
      Done: r.done ? 'Yes' : 'No'
    }));

    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(cleanedData);
    const workbook: XLSX.WorkBook = { Sheets: { 'Reminders': worksheet }, SheetNames: ['Reminders'] };
    const buffer: any = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
    const blob: Blob = new Blob([buffer], { type: 'application/octet-stream' });
    FileSaver.saveAs(blob, 'reminders.xlsx');
  }

  exportToCSV(): void {
    const cleanedData = this.filteredReminders.map(r => ({
      Company: r.companyName,
      Amount: r.amount,
      Description: r.description,
      Done: r.done ? 'Yes' : 'No'
    }));

    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(cleanedData);
    const csv: string = XLSX.utils.sheet_to_csv(worksheet);
    const blob: Blob = new Blob([csv], { type: 'text/csv;charset=utf-8;' });
    FileSaver.saveAs(blob, 'reminders.csv');
  }

  exportToPDF(): void {
    const doc = new jsPDF();
    const rows = this.filteredReminders.map(r => [
      r.companyName ?? '',
      String(r.amount ?? ''),
      r.description ?? '',
      r.done ? 'Yes' : 'No'
    ]);

    autoTable(doc, {
      head: [['Company', 'Amount', 'Description', 'Done']],
      body: rows
    });

    doc.save('reminders.pdf');
  }

  edit(reminder: Reminder) {
    this.editingReminder = { ...reminder };

    // Scroll to the edit form after it's rendered
    setTimeout(() => {
      this.editFormElement?.nativeElement.scrollIntoView({ behavior: 'smooth' });
    }, 0);
  }

  cancelEdit() {
    this.editingReminder = null;
  }

  update() {
    if (this.editingReminder) {
      this.store.dispatch(updateReminder({ reminder: this.editingReminder }));
      this.editingReminder = null;
    }
  }
}
